// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Does not inject properties.
/// </summary>
internal class NoInjection : IInjectionBehavior
{
    /// <summary>
    /// Inject dependencies in the required fashion.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="target">The target.</param>
    public void InjectDependencies(IComponentContext context, object target)
    {
    }
}
