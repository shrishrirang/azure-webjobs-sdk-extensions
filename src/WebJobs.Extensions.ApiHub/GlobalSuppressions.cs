// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubConfiguration.#Initialize(Microsoft.Azure.WebJobs.Host.Config.ExtensionConfigContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.Common.GenericOutStringFileBinding`2.#BindAsync(Microsoft.Azure.WebJobs.Host.Bindings.BindingContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubJobHostConfigurationExtensions.#UseApiHub(Microsoft.Azure.WebJobs.JobHostConfiguration,Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubConfiguration)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.Common.GenericStreamBindingProvider`2.#TryCreateAsync(Microsoft.Azure.WebJobs.Host.Bindings.BindingProviderContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.Common.GenericFileBinding`2.#BindAsync(Microsoft.Azure.WebJobs.Host.Bindings.BindingContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.Common.GenericFileTriggerBindingProvider`2+GenericTriggerbinding.#CreateListenerAsync(Microsoft.Azure.WebJobs.Host.Listeners.ListenerFactoryContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Microsoft.Azure.WebJobs.ApiHubFileAttribute")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Microsoft.Azure.WebJobs.ApiHubJobHostConfigurationExtensions.#UseApiHub(Microsoft.Azure.WebJobs.JobHostConfiguration,Microsoft.Azure.WebJobs.ApiHubConfiguration)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Microsoft.Azure.WebJobs.ApiHubFileAttribute")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Microsoft.Azure.WebJobs.ApiHubFileTriggerAttribute")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IFolderItem", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener.#StartAsync(System.Threading.CancellationToken)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RootPath", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener.#StartAsync(System.Threading.CancellationToken)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "taskIgnore", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener.#Cancel()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener+ApiHubFileInfo.#FunctionName")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener+ApiHubFileInfo.#FilePath")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.Azure.WebJobs.Extensions.ApiHub.ApiHubListener+ApiHubFileInfo.#Connection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Microsoft.Azure.WebJobs.ApiHubConfiguration.#BuildListener(Microsoft.Azure.WebJobs.JobHostConfiguration,Microsoft.Azure.WebJobs.ApiHubFileTriggerAttribute,System.String,Microsoft.Azure.WebJobs.Host.Executors.ITriggeredFunctionExecutor,Microsoft.Azure.WebJobs.Host.TraceWriter)")]
