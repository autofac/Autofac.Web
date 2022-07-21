// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web;

namespace Autofac.Integration.Web
{
    /// <summary>
    /// HTTP Module that disposes of Autofac-created components when processing for
    /// a request completes.
    /// </summary>
    public class ContainerDisposalModule : IHttpModule
    {
        private IContainerProviderAccessor? _containerProviderAccessor;

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application.</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _containerProviderAccessor = context as IContainerProviderAccessor;
            if (_containerProviderAccessor == null)
            {
                throw new InvalidOperationException(ContainerDisposalModuleResources.ApplicationMustImplementAccessor);
            }

            var wrapper = new EventHandlerTaskAsyncHelper(OnEndRequest);

            context.AddOnEndRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        /// <summary>
        /// Dispose of the per-request container.
        /// </summary>
        [SuppressMessage("CA1849", "CA1849", Justification = "If the value task is already completed, getting the result synchronously isn't a problem.")]
        private Task OnEndRequest(object sender, EventArgs e)
        {
            var cp = _containerProviderAccessor?.ContainerProvider;
            if (cp == null)
            {
                throw new InvalidOperationException(ContainerDisposalModuleResources.ContainerProviderNull);
            }

            var valueTask = cp.EndRequestLifetime();

            // https://github.com/dotnet/aspnetcore/blob/main/src/Shared/ValueTaskExtensions/ValueTaskExtensions.cs#L13

            // Try to avoid the allocation from AsTask
            if (valueTask.IsCompletedSuccessfully)
            {
                // Signal consumption to the IValueTaskSource
                valueTask.GetAwaiter().GetResult();
                return Task.CompletedTask;
            }
            else
            {
                return valueTask.AsTask();
            }
        }
    }
}
