// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Injects any resolvable properties.
/// </summary>
internal class PropertyInjection : PageInjectionBehavior
{
    /// <summary>
    /// Override to return a closure that injects properties into a target.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The injector.</returns>
    protected override Func<object, object> GetInjector(IComponentContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.InjectProperties<object>;
    }
}
