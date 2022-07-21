// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Dependency injection module that will always inject any resolvable
/// properties.
/// </summary>
public class PropertyInjectionModule : DependencyInjectionModule
{
    private readonly IInjectionBehavior _injectProperties = new PropertyInjection();

    /// <summary>
    /// Override to customize injection behavior based on HTTP Handler type.
    /// </summary>
    /// <param name="handlerType">Type of the handler.</param>
    /// <returns>The injection behavior.</returns>
    protected override IInjectionBehavior GetInjectionBehaviorForHandlerType(Type handlerType)
    {
        if (handlerType == null)
        {
            throw new ArgumentNullException(nameof(handlerType));
        }

        return _injectProperties;
    }
}
