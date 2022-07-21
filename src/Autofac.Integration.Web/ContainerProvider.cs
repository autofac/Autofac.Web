// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        private readonly Action<ContainerBuilder>? _requestLifetimeConfiguration;

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
        /// <param name="requestLifetimeConfiguration">
        /// An action that will be executed when building
        /// the per-request lifetime. The components visible within the request can be
        /// customized here.
        /// </param>
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
