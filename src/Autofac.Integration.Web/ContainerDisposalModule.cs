﻿// Contributed by Nicholas Blumhardt 2008-01-28
// Copyright © 2011 Autofac Contributors
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

namespace Autofac.Integration.Web
{
    /// <summary>
    /// HTTP Module that disposes of Autofac-created components when processing for
    /// a request completes.
    /// </summary>
    public class ContainerDisposalModule : IHttpModule
    {
        private IContainerProviderAccessor _containerProviderAccessor;

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
                throw new ArgumentNullException(nameof(context));

            _containerProviderAccessor = context as IContainerProviderAccessor;
            if (_containerProviderAccessor == null)
                throw new InvalidOperationException(ContainerDisposalModuleResources.ApplicationMustImplementAccessor);

            context.EndRequest += OnEndRequest;
        }

        /// <summary>
        /// Dispose of the per-request container.
        /// </summary>
        private void OnEndRequest(object sender, EventArgs e)
        {
            var cp = _containerProviderAccessor.ContainerProvider;
            if (cp == null)
                throw new InvalidOperationException(ContainerDisposalModuleResources.ContainerProviderNull);
            cp.EndRequestLifetime();
        }
    }
}
