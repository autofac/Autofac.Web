// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web
{
    /// <summary>
    /// Provides global and per-request Autofac containers in an
    /// ASP.NET application.
    /// </summary>
    public interface IContainerProvider
    {
        /// <summary>
        /// Dispose of the current request's container, if it has been
        /// instantiated.
        /// </summary>
        ValueTask EndRequestLifetime();

        /// <summary>
        /// Gets the global, application-wide container.
        /// </summary>
        IContainer ApplicationContainer { get; }

        /// <summary>
        /// Gets the lifetime used to manage components for processing the
        /// current request.
        /// </summary>
        ILifetimeScope RequestLifetime { get; }
    }
}
