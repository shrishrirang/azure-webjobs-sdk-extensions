﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace Microsoft.Azure.WebJobs.Extensions.SqlQueue
{
    internal class SqlQueueTriggerAttributeBindingProvider : ITriggerBindingProvider
    {
        public SqlQueueTriggerAttributeBindingProvider()
        {
            
        }
        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ParameterInfo parameter = context.Parameter;
            SqlQueueTriggerAttribute attribute = parameter.GetCustomAttribute<SqlQueueTriggerAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            // TODO: Define the types your binding supports here
            if (parameter.ParameterType != typeof(SqlQueueTriggerValue) &&
                parameter.ParameterType != typeof(string))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind SqlQueueTriggerAttribute to type '{0}'.", parameter.ParameterType));
            }

            return Task.FromResult<ITriggerBinding>(new SqlQueueTriggerBinding(context.Parameter));
        }

        private class SqlQueueTriggerBinding : ITriggerBinding
        {
            private readonly ParameterInfo _parameter;
            private readonly IReadOnlyDictionary<string, Type> _bindingContract;

            public SqlQueueTriggerBinding(ParameterInfo parameter)
            {
                _parameter = parameter;
                _bindingContract = CreateBindingDataContract();
            }

            public IReadOnlyDictionary<string, Type> BindingDataContract
            {
                get { return _bindingContract; }
            }

            public Type TriggerValueType
            {
                get { return typeof(SqlQueueTriggerValue); }
            }

            public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
            {
                // TODO: Perform any required conversions on the value
                // E.g. convert from Dashboard invoke string to our trigger
                // value type
                SqlQueueTriggerValue queueTriggerValue = value as SqlQueueTriggerValue;
                IValueBinder valueBinder = new SqlQueueValueBinder(_parameter, queueTriggerValue);
                return Task.FromResult<ITriggerData>(new TriggerData(valueBinder, GetBindingData(queueTriggerValue)));
            }

            public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
            {
                return Task.FromResult<IListener>(new Listener(context.Executor));
            }

            public ParameterDescriptor ToParameterDescriptor()
            {
                return new SqlQueueTriggerParameterDescriptor
                {
                    Name = _parameter.Name,
                    DisplayHints = new ParameterDisplayHints
                    {
                        // TODO: Customize your Dashboard display strings
                        Prompt = "Sample",
                        Description = "Sample trigger fired",
                        DefaultValue = "Sample"
                    }
                };
            }

            private IReadOnlyDictionary<string, object> GetBindingData(SqlQueueTriggerValue value)
            {
                Dictionary<string, object> bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                bindingData.Add("SampleTrigger", value);

                // TODO: Add any additional binding data

                return bindingData;
            }

            private IReadOnlyDictionary<string, Type> CreateBindingDataContract()
            {
                Dictionary<string, Type> contract = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
                contract.Add("SampleTrigger", typeof(SqlQueueTriggerValue));

                // TODO: Add any additional binding contract members

                return contract;
            }

            private class SqlQueueTriggerParameterDescriptor : TriggerParameterDescriptor
            {
                public override string GetTriggerReason(IDictionary<string, string> arguments)
                {
                    // TODO: Customize your Dashboard display string
                    return string.Format("Sample trigger fired at {0}", DateTime.Now.ToString("o"));
                }
            }

            private class SqlQueueValueBinder : ValueBinder
            {
                private readonly object _value;

                public SqlQueueValueBinder(ParameterInfo parameter, SqlQueueTriggerValue value)
                    : base(parameter.ParameterType)
                {
                    _value = value;
                }

                public override Task<object> GetValueAsync()
                {
                    // TODO: Perform any required conversions
                    if (Type == typeof(string))
                    {
                        return Task.FromResult<object>(_value.ToString());
                    }
                    return Task.FromResult(_value);
                }

                public override string ToInvokeString()
                {
                    // TODO: Customize your Dashboard invoke string
                    return "Sample";
                }
            }

            private class Listener : IListener
            {
                private ITriggeredFunctionExecutor _executor;
                private System.Timers.Timer _timer;

                public Listener(ITriggeredFunctionExecutor executor)
                {
                    _executor = executor;

                    // TODO: For this sample, we're using a timer to generate
                    // trigger events. You'll replace this with your event source.
                    _timer = new System.Timers.Timer(5 * 1000)
                    {
                        AutoReset = true
                    };
                    _timer.Elapsed += OnTimer;
                }

                public Task StartAsync(CancellationToken cancellationToken)
                {
                    // TODO: Start monitoring your event source
                    _timer.Start();
                    return Task.FromResult(true);
                }

                public Task StopAsync(CancellationToken cancellationToken)
                {
                    // TODO: Stop monitoring your event source
                    _timer.Stop();
                    return Task.FromResult(true);
                }

                public void Dispose()
                {
                    // TODO: Perform any final cleanup
                    _timer.Dispose();
                }

                public void Cancel()
                {
                    // TODO: cancel any outstanding tasks initiated by this listener
                }

                private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
                {
                    // TODO: When you receive new events from your event source,
                    // invoke the function executor
                    TriggeredFunctionData input = new TriggeredFunctionData
                    {
                        TriggerValue = new SqlQueueTriggerValue()
                    };
                    _executor.TryExecuteAsync(input, CancellationToken.None).Wait();
                }
            }
        }
    }
}
