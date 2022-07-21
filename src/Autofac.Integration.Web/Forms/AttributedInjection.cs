// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Injects dependencies into request handlers and pages that have been
    /// decorated with the [InjectProperties] or [InjectUnsetProperties]
    /// attributes.
    /// </summary>
    internal class AttributedInjection : PageInjectionBehavior
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

            return target =>
            {
                var targetType = target.GetType();
                if (targetType.GetCustomAttributes(typeof(InjectPropertiesAttribute), true).Length > 0)
                {
                    return context.InjectProperties(target);
                }
                else if (targetType.GetCustomAttributes(typeof(InjectUnsetPropertiesAttribute), true).Length > 0)
                {
                    return context.InjectUnsetProperties(target);
                }
                else
                {
                    return target;
                }
            };
        }
    }
}
