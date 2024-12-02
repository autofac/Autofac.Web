// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac.Core;

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Provides default property selector that applies appropriate filters to ensure only
/// public settable properties are selected (including filtering for value types and indexed
/// properties).
/// </summary>
public class RequiredPropertySelector : IPropertySelector
{
    /// <summary>
    /// Gets an instance of RequiredPropertySelector that will cause values to be overwritten.
    /// </summary>
    internal static IPropertySelector Default { get; } = new RequiredPropertySelector();

    /// <summary>
    /// Provides [RequiredMember] filtering to determine if property should be injected by rejecting
    /// non-public settable properties.
    /// </summary>
    /// <param name="propertyInfo">Property to be injected.</param>
    /// <param name="instance">Instance that has the property to be injected.</param>
    /// <returns>Whether property should be injected.</returns>
    [SuppressMessage("CA1031", "CA1031", Justification = "Issue #799: If getting the property value throws an exception then assume it's set and skip it.")]
    public virtual bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        if (propertyInfo == null)
        {
            return false;
        }

        if (!propertyInfo.CanWrite || propertyInfo.SetMethod?.IsPublic != true)
        {
            return false;
        }

        return propertyInfo.HasRequiredMemberAttribute();
    }
}
