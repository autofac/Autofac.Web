// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web.UI;

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Assists with the construction of page injectors.
/// </summary>
internal abstract class PageInjectionBehavior : IInjectionBehavior
{
    /// <summary>
    /// Inject dependencies in the required fashion.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="target">The target.</param>
    public void InjectDependencies(IComponentContext context, object target)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        var injector = GetInjector(context);

        DoInjection(injector, target);
    }

    /// <summary>
    /// Override to return a closure that injects properties into a target.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The injector.</returns>
    protected abstract Func<object, object> GetInjector(IComponentContext context);

    /// <summary>
    /// Does the injection using a supplied injection function.
    /// </summary>
    /// <param name="injector">The injector.</param>
    /// <param name="target">The target.</param>
    private static void DoInjection(Func<object, object> injector, object target)
    {
        if (injector == null)
        {
            throw new ArgumentNullException(nameof(injector));
        }

        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        injector(target);

        if (target is Page page)
        {
            page.PreLoad += (s, e) => InjectUserControls(injector, page);
        }
    }

    private static void InjectUserControls(Func<object, object> injector, Control parent)
    {
        if (injector == null)
        {
            throw new ArgumentNullException(nameof(injector));
        }

        if (parent == null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        if (parent.Controls == null)
        {
            return;
        }

        foreach (Control control in parent.Controls)
        {
            if (control is UserControl uc)
            {
                injector(uc);
            }

            InjectUserControls(injector, control);
        }
    }
}
