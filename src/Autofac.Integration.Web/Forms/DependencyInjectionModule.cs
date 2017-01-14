﻿// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// http://autofac.org
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
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Base for classes that inject dependencies into HTTP Handlers.
    /// </summary>
    public abstract class DependencyInjectionModule : ModuleBase, IHttpModule
    {




        IInjectionBehavior _noInjection = new NoInjection();
        IInjectionBehavior _propertyInjection = new PropertyInjection();
        IInjectionBehavior _unsetPropertyInjection = new UnsetPropertyInjection();

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpApplication = context;
            AddContainerProviderAccessors(context);
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }



        /// <summary>
        /// Called before the request handler is executed so that dependencies
        /// can be injected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var handler = HttpApplication.Context.CurrentHandler;

            if (handler != null)
            {
                var typeName = handler.GetType().BaseType.FullName;
                var constraint = ContainerProviderAccessors
                    .FirstOrDefault(c => string.IsNullOrEmpty(c.NamespaceRegexPattern) || Regex.IsMatch(typeName, c.NamespaceRegexPattern));
                if (constraint != null)
                {
                    var injectionBehavior = GetInjectionBehavior(handler);
                    var cp = constraint.ProviderAccessor.ContainerProvider;
                    if (cp == null)
                        throw new InvalidOperationException(ContainerDisposalModuleResources.ContainerProviderNull);
                    injectionBehavior.InjectDependencies(cp.RequestLifetime, handler);
                }
            }
        }

        /// <summary>
        /// Internal for testability outside of a web application.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>The injection behavior.</returns>
        protected internal IInjectionBehavior GetInjectionBehavior(IHttpHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (handler is DefaultHttpHandler)
            {
                return _noInjection;
            }
            else
            {
                var handlerType = handler.GetType();
                return GetInjectionBehaviorForHandlerType(handlerType);
            }
        }

        /// <summary>
        /// A behavior that does not inject dependencies.
        /// </summary>
        protected IInjectionBehavior NoInjection
        {
            get { return _noInjection; }
        }

        /// <summary>
        /// A behavior that injects resolvable dependencies.
        /// </summary>
        protected IInjectionBehavior PropertyInjection
        {
            get { return _propertyInjection; }
        }

        /// <summary>
        /// A behavior that injects unset, resolvable dependencies.
        /// </summary>
        protected IInjectionBehavior UnsetPropertyInjection
        {
            get { return _unsetPropertyInjection; }
        }

        /// <summary>
        /// Override to customize injection behavior based on HTTP Handler type.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>The injection behavior.</returns>
        protected abstract IInjectionBehavior GetInjectionBehaviorForHandlerType(Type handlerType);
    }
}


