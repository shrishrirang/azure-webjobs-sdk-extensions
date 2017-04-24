// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SqlQueue;
using Sample.Extension;

namespace ExtensionsSample
{
    public static class SqlQueueSamples
    {
        public static void SqlQueue_Trigger_Basic([SqlQueueTrigger(null)] string queueMessage)
        {
            Console.WriteLine(queueMessage);
        }

        public static void SqlQueue_Trigger_With_Metadata([SqlQueueTrigger(null)] string queueMessage, SqlQueueTriggerValue sampleTrigger)
        {
            Console.WriteLine(queueMessage);
        }
    }
}
