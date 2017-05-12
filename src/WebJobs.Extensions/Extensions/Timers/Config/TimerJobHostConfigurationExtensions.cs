﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs.Extensions.Timers.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Microsoft.Azure.WebJobs
{
    /// <summary>
    /// Extension class used to register timer extensions.
    /// </summary>
    public static class TimerJobHostConfigurationExtensions
    {
        /// <summary>
        /// Enables use of the Timer extensions
        /// </summary>
        /// <param name="config">The <see cref="JobHostConfiguration"/> to configure.</param>
        public static void UseTimers(this JobHostConfiguration config)
        {
            UseTimers(config, new TimersConfiguration());
        }

        /// <summary>
        /// Enables use of the Timer extensions
        /// </summary>
        /// <param name="config">The <see cref="JobHostConfiguration"/> to configure.</param>
        /// <param name="timersConfig">The <see cref="TimersConfiguration"/> to use.</param>
        public static void UseTimers(this JobHostConfiguration config, TimersConfiguration timersConfig)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (timersConfig == null)
            {
                throw new ArgumentNullException("timersConfig");
            }

            config.RegisterExtensionConfigProvider(new TimersExtensionConfig(timersConfig));
        }

        private class TimersExtensionConfig : IExtensionConfigProvider
        {
            private readonly TimersConfiguration _config;

            public TimersExtensionConfig(TimersConfiguration config)
            {
                _config = config;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                if (_config.ScheduleMonitor == null)
                {
                    // TODO: Come up with a better way to check for standalone mode. Also, review whether to use FileSystemScheduleMonitor for standalone mode
                    if (String.CompareOrdinal(Environment.GetEnvironmentVariable("AzureWebJobsScriptMode"), "standalone") == 0)
                    {
                        _config.ScheduleMonitor = new FileSystemScheduleMonitor();
                    }
                    else
                    {
                        _config.ScheduleMonitor = new StorageScheduleMonitor(context.Config, context.Trace);
                    }
                }

                context.Config.RegisterBindingExtension(new TimerTriggerAttributeBindingProvider(_config, context.Config.NameResolver, context.Trace));
            }
        }
    }
}
