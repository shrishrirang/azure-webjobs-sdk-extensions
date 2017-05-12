// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Extensions.SqlQueue;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Microsoft.Azure.WebJobs
{
    public static class SqlQueueJobHostConfigurationExtensions
    {
        public static void UseSqlQueue(this JobHostConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Register our extension configuration provider
            config.RegisterExtensionConfigProvider(new SqlQueueExtensionConfig());
        }
    }
}
