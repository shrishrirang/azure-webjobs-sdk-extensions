// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host;

namespace Microsoft.Azure.WebJobs
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SqlQueueTriggerAttribute : Attribute
    {
        private const string ConnectionStringName = "SqlQueue";

        public SqlQueueTriggerAttribute()
        {
            MessageDataType = "VarChar(Max)"; // default message data type
            ConnectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringName);
        }

        public string ConnectionString { get; private set; }

        public string DatabaseName { get; set; }

        public string QueueName { get; set; }

        public string ContractName { get; set; }

        public string MessageDataType { get; set; }
    }
}
