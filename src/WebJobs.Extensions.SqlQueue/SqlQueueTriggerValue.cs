// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.WebJobs.Extensions.SqlQueue
{
    public class SqlQueueTriggerValue
    {
        public SqlQueueTriggerValue() // FIXME: Fix name
        {
        }

        public string Value { get; set; }

        // TODO: Define the default type that your trigger binding
        // binds to (the type representing the trigger event).

        public override string ToString()
        {
            return Value;
        }
    }
}
