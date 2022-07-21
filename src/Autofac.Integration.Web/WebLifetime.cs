// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Core.Lifetime;

namespace Autofac.Integration.Web
{
    /// <summary>
    /// Constants used to tag liftime scopes within standard Autofac web applications.
    /// </summary>
    public static class WebLifetime
    {
        /// <summary>
        /// Application root lifetime scope tag. Use <see cref="Autofac.Core.Lifetime.LifetimeScope.RootTag"/> instead.
        /// </summary>
        [Obsolete("Use Autofac.Core.LifetimeScope.RootTag for application lifetime scope tagging.")]
        public static readonly object Application = LifetimeScope.RootTag;
    }
}
