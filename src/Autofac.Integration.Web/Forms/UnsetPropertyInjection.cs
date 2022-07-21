// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Injects resolvable properties that do not already have a value.
/// </summary>
internal class UnsetPropertyInjection : PageInjectionBehavior
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

        return context.InjectUnsetProperties<object>;
    }
}
