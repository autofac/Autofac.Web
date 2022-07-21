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
using System.Threading.Tasks;
using System.Web;
using Autofac.Core.Lifetime;

namespace Autofac.Integration.Web
{
    /// <summary>
    /// Provides application-wide and per-request containers.
    /// </summary>
    public class ContainerProvider : IContainerProvider
    {
        private readonly Action<ContainerBuilder> _requestLifetimeConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerProvider"/> class.
        /// </summary>
        /// <param name="applicationContainer">The application container.</param>
        public ContainerProvider(IContainer applicationContainer)
        {
            ApplicationContainer = applicationContainer ?? throw new ArgumentNullException(nameof(applicationContainer));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerProvider"/> class.
        /// </summary>
        /// <param name="applicationContainer">The application container.</param>
        /// <param name="requestLifetimeConfiguration">An action that will be executed when building
        /// the per-request lifetime. The components visible within the request can be
        /// customised here.</param>
        public ContainerProvider(IContainer applicationContainer, Action<ContainerBuilder> requestLifetimeConfiguration)
            : this(applicationContainer)
        {
            _requestLifetimeConfiguration = requestLifetimeConfiguration ?? throw new ArgumentNullException(nameof(requestLifetimeConfiguration));
        }

        /// <summary>
        /// Dispose of the current request's container, if it has been
        /// instantiated.
        /// </summary>
        public ValueTask EndRequestLifetime()
        {
            var rc = AmbientRequestLifetime;
            return rc == null ? default : rc.DisposeAsync();
        }

        /// <summary>
        /// Gets the global, application-wide container.
        /// </summary>
        public IContainer ApplicationContainer { get; }

        /// <summary>
        /// Gets the container used to manage components for processing the
        /// current request.
        /// </summary>
        public ILifetimeScope RequestLifetime
        {
            get
            {
                var result = AmbientRequestLifetime;
                if (result == null)
                {
                    result = _requestLifetimeConfiguration == null ?
                        ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag) :
                        ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, _requestLifetimeConfiguration);

                    AmbientRequestLifetime = result;
                }

                return result;
            }
        }

        private static ILifetimeScope AmbientRequestLifetime
        {
            get
            {
                return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)];
            }

            set
            {
                HttpContext.Current.Items[typeof(ILifetimeScope)] = value;
            }
        }
    }
}
