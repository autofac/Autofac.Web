// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using Autofac.Core;
using Autofac.Util.Cache;

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Injects dependencies into request handlers and pages that have been
/// decorated with the [InjectProperties] or [InjectUnsetProperties]
/// attributes.
/// </summary>
internal class AttributedInjection : PageInjectionBehavior
{
    // https://github.com/autofac/Autofac/blob/d2ed00a046c2d47374ee7a1517fabf2c4eb80c81/src/Autofac/Core/InternalReflectionCaches.cs#L86
    private static readonly ConcurrentDictionary<Type, bool> HasRequiredMemberAttributeCache = ReflectionCacheSet.Shared
        .GetOrCreateCache<ReflectionCacheDictionary<Type, bool>>(nameof(HasRequiredMemberAttribute));

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
            else if (HasRequiredMemberAttribute(targetType))
            {
                return context.InjectProperties(target, RequiredPropertySelector.Default);
            }
            else
            {
                return target;
            }
        };
    }

    // https://github.com/autofac/Autofac/blob/d2ed00a046c2d47374ee7a1517fabf2c4eb80c81/src/Autofac/Core/Activators/Reflection/ReflectionActivator.cs#L85
    private static bool HasRequiredMemberAttribute(Type type)
    {
        // The RequiredMemberAttribute (may)* have Inherit = false on its AttributeUsage options,
        // so walk the tree.
        // (*): see `HasRequiredMemberAttribute` doc for why we dont really know much about the concrete attribute.
        return HasRequiredMemberAttributeCache.GetOrAdd(type, t =>
        {
            for (var currentType = t; currentType != null && currentType != typeof(object); currentType = currentType.BaseType)
            {
                if (currentType.HasRequiredMemberAttribute())
                {
                    return true;
                }
            }

            return false;
        });
    }
}
