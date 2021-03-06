﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace Microsoft.Azure.WebJobs.Extensions.SqlQueue
{
    internal class SqlQueueAttributeBindingProvider : IBindingProvider
    {
        public SqlQueueAttributeBindingProvider()
        {
            
        }
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // Determine whether we should bind to the current parameter
            ParameterInfo parameter = context.Parameter;
            SqlQueueAttribute attribute = parameter.GetCustomAttribute<SqlQueueAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            // TODO: Include any other parameter types this binding supports in this check
            IEnumerable<Type> supportedTypes = StreamValueBinder.GetSupportedTypes(FileAccess.ReadWrite);
            if (!ValueBinder.MatchParameterType(context.Parameter, supportedTypes))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, 
                    "Can't bind SqlQueueAttribute to type '{0}'.", parameter.ParameterType));
            }

            return Task.FromResult<IBinding>(new SqlQueueBinding(parameter));
        }

        private class SqlQueueBinding : IBinding
        {
            private readonly ParameterInfo _parameter;

            public SqlQueueBinding(ParameterInfo parameter)
            {
                _parameter = parameter;
            }

            public bool FromAttribute
            {
                get { return true; }
            }

            public Task<IValueProvider> BindAsync(BindingContext context)
            {
                return Task.FromResult<IValueProvider>(new SqlQueueValueBinder(_parameter));
            }

            public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            {
                // TODO: Perform any conversions on the incoming value
                // E.g., it may be a Dashboard invocation string value
                return Task.FromResult<IValueProvider>(new SqlQueueValueBinder(_parameter));
            }

            public ParameterDescriptor ToParameterDescriptor()
            {
                return new ParameterDescriptor
                {
                    Name = _parameter.Name,
                    DisplayHints = new ParameterDisplayHints
                    {
                        // TODO: Define your Dashboard integration strings here.
                        Description = "Sample",
                        DefaultValue = "Sample",
                        Prompt = "Please enter a Sample value"
                    }
                };
            }

            // TODO: Implement your binder. You can derive from StreamValueBinder
            // to get a bunch of built in bindings for free (mapping from string to
            // other param types), or you may chose to derive from ValueBinder and
            // implement everything yourself
            private class SqlQueueValueBinder : StreamValueBinder
            {
                private readonly ParameterInfo _parameter;

                public SqlQueueValueBinder(ParameterInfo parameter)
                    : base(parameter)
                {
                    _parameter = parameter;
                }

                protected override Stream GetStream()
                {
                    return new MemoryStream();
                }

                public override string ToInvokeString()
                {
                    // TODO: Return the string that should be shown in the Dashboard
                    // for this parameter
                    return "Sample";
                }
            }
        }
    }
}
