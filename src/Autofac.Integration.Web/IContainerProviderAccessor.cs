// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web;

/// <summary>
/// Implemented on a type (i.e. HttpApplication) that maintains a container provider
/// for injecting dependencies into web requests.
/// </summary>
public interface IContainerProviderAccessor
{
    /// <summary>
    /// Gets the container provider.
    /// </summary>
    /// <value>The container provider.</value>
    IContainerProvider ContainerProvider { get; }
}
