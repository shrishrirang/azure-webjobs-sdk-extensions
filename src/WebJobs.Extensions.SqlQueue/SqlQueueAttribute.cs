// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Description;

namespace Microsoft.Azure.WebJobs
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SqlQueueAttribute : Attribute
    {
        public SqlQueueAttribute(string path)
        {
            Path = path;
        }

        // TODO: Define your domain specific values here
        public string Path { get; private set; }
    }
}
