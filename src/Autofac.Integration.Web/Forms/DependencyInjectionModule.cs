﻿// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// https://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Web;

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Base for classes that inject dependencies into HTTP Handlers.
    /// </summary>
    public abstract class DependencyInjectionModule : IHttpModule
    {
        private IContainerProviderAccessor _containerProviderAccessor;
        private HttpApplication _httpApplication;

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
            _httpApplication = context ?? throw new ArgumentNullException(nameof(context));
            _containerProviderAccessor = context as IContainerProviderAccessor;

            if (_containerProviderAccessor == null)
                throw new InvalidOperationException(DependencyInjectionModuleResources.ApplicationMustImplementAccessor);

            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        /// <summary>
        /// Called before the request handler is executed so that dependencies
        /// can be injected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var handler = _httpApplication.Context.CurrentHandler;
            if (handler != null)
            {
                var injectionBehavior = GetInjectionBehavior(handler);
                var cp = _containerProviderAccessor.ContainerProvider;
                if (cp == null)
                    throw new InvalidOperationException(ContainerDisposalModuleResources.ContainerProviderNull);
                injectionBehavior.InjectDependencies(cp.RequestLifetime, handler);
            }
        }

        /// <summary>
        /// Internal for testability outside of a web application.
        /// </summary>
        /// <param name="handler">The handler on which to inject dependencies.</param>
        /// <returns>The injection behavior.</returns>
        protected internal IInjectionBehavior GetInjectionBehavior(IHttpHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (handler is DefaultHttpHandler)
            {
                return NoInjection;
            }
            else
            {
                var handlerType = handler.GetType();
                return GetInjectionBehaviorForHandlerType(handlerType);
            }
        }

        /// <summary>
        /// Gets a behavior that does not inject dependencies.
        /// </summary>
        protected IInjectionBehavior NoInjection { get; } = new NoInjection();

        /// <summary>
        /// Gets a behavior that injects resolvable dependencies.
        /// </summary>
        protected IInjectionBehavior PropertyInjection { get; } = new PropertyInjection();

        /// <summary>
        /// Gets a behavior that injects unset, resolvable dependencies.
        /// </summary>
        protected IInjectionBehavior UnsetPropertyInjection { get; } = new UnsetPropertyInjection();

        /// <summary>
        /// Override to customize injection behavior based on HTTP Handler type.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>The injection behavior.</returns>
        protected abstract IInjectionBehavior GetInjectionBehaviorForHandlerType(Type handlerType);
    }
}
