// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Azure.WebJobs.Extensions.SqlQueue;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;

namespace Microsoft.Azure.WebJobs.Extensions
{
    // Listens to new messages in the SQL service broker queue.
    internal class SqlQueueListener : IListener
    {
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly SqlQueueTriggerAttribute _triggerAttribute;

        private Task _pollTask;
        private CancellationTokenSource _cancellationTokenSource;

        // All units in milli seconds
        private const int ReadWaitPeriod = 10 * 1000;
        private const int DelayWhenQueueEmpty = 5 * 1000;
        private const int DelayWhenQueueNotEmpty = 2 * 1000; // > 0 as we don't want any webjob to starve when multiple jobs are triggered by the same queue

        // Defines the number of messages to read from the queue in each batch.
        // This is configurable but is set to 1 as we want the sql transactions to be short.
        // Besides, failure to process any on of the fetched messages will cause rollback of the transaction.
        // This means, all the fetched messages would be put back in the queue, but the webjobs would have been run already. This is messy.
        // Keeping it simple and clean by just fetching 1 message at a time from the queue.
        private const int MessageBatchSize = 1;

        public SqlQueueListener(ITriggeredFunctionExecutor executor, SqlQueueTriggerAttribute triggerAttribute)
        {
            _executor = executor;
            _triggerAttribute = triggerAttribute;
            _pollTask = Task.CompletedTask;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Fail if polling is already in progress
            if (_pollTask != null && !_pollTask.IsCompleted)
            {
                throw new InvalidOperationException($"{this.GetType().FullName} has been started already!");
            }

            // Create a cancellation token source for later use and start polling
            _cancellationTokenSource = new CancellationTokenSource();
            _pollTask = PollQueueAsync(_cancellationTokenSource.Token);

            // Polling has been started. Return a completed task.
            return Task.CompletedTask;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // If polling is complete or if it was never started, there is nothing to be stopped. 
            if (_pollTask == null || _pollTask.IsCompleted)
            {
                return;
            }

            Debug.Assert(_cancellationTokenSource != null);

            // Cancel the task 
            _cancellationTokenSource.Cancel();

            // Wait for the canceled task to be complete before we return
            try
            {
                await _pollTask;
            }
            catch (OperationCanceledException)
            {
                // NOP. We canceled the task ourselves. This exception was expected. 
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Cancel()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Poll the SQL service broker queue for new messages.
        // The implementation is a mix of long poll and delayed retries.
        private async Task PollQueueAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int messageCount = 0;

                // Database name is optional in the connection string
                using (SqlConnection connection = new SqlConnection(_triggerAttribute.ConnectionString))
                {
                    await OpenConnectionAsync(connection, cancellationToken);

                    using (SqlCommand readCommand = connection.CreateCommand())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                InitializeReadQueueCommand(readCommand, transaction);

                                messageCount = await ProcessQueueAsync(readCommand, cancellationToken);

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // FIXME: Log
                                transaction.Rollback();

                                // Some Sql calls fail with a SqlException (instead of an OperationCanceledException) when the task is cancelled.
                                // So we check if the exception is a SqlException and if the task has been canceled already.
                                // If so, we assume that the SqlException is a result of task cancellation and throw an OperationCanceledException.
                                if (ex is SqlException)
                                {
                                    cancellationToken.ThrowIfCancellationRequested();
                                }

                                throw;
                            }
                        }
                    }
                }

                // Determine wait period based on whether or not we found messages in the queue
                var delay = messageCount == 0 ? DelayWhenQueueEmpty : DelayWhenQueueNotEmpty;
                await Task.Delay(delay, cancellationToken);
            }
        }

        private async Task OpenConnectionAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            await connection.OpenAsync(cancellationToken);

            // Database name need not be specified explicitly, if it is already specified in the connection string
            if (!string.IsNullOrWhiteSpace(_triggerAttribute.DatabaseName))
            {
                connection.ChangeDatabase(_triggerAttribute.DatabaseName);
            }
        }

        private async Task<int> ProcessQueueAsync(SqlCommand readCommand, CancellationToken cancellationToken)
        {
            using (SqlDataReader reader = await readCommand.ExecuteReaderAsync(cancellationToken))
            {
                return await ProcessMessagesAsync(reader, cancellationToken);
            }
        }

        private async Task<int> ProcessMessagesAsync(SqlDataReader reader, CancellationToken cancellationToken)
        {
            int messageCount = 0;

            // Loop through all the retrieved queue messages
            while (await reader.ReadAsync(cancellationToken))
            {
                messageCount++;

                if (reader.FieldCount != 1)
                {
                    throw new InvalidOperationException($"Row contains {reader.FieldCount} columns. Expected exactly 1 column to be present");
                }

                string message;
                if (await reader.IsDBNullAsync(0, cancellationToken))
                {
                    message = null;
                }
                else
                {
                    message = reader.GetString(0);
                }

                TriggeredFunctionData triggerData = new TriggeredFunctionData
                {
                    TriggerValue = new SqlQueueTriggerValue
                    {
                        Value = message
                    }
                };

                await _executor.TryExecuteAsync(triggerData, CancellationToken.None);
            }

            return messageCount;
        }

        private void InitializeReadQueueCommand(SqlCommand command, SqlTransaction transaction)
        {
            command.CommandText = $"WAITFOR (RECEIVE TOP({MessageBatchSize}) CONVERT({_triggerAttribute.MessageDataType}, message_body) FROM {_triggerAttribute.QueueName}), TIMEOUT {ReadWaitPeriod}";
            command.CommandType = CommandType.Text;
            command.Transaction = transaction;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Nothing to dispose as of now. 
            }
        }
    }
}
