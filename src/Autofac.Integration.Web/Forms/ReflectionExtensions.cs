// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

// https://github.com/autofac/Autofac/blob/f5346a7bb0115a3a5ef9a2d83dbc01ae56d8cf1f/src/Autofac/Util/ReflectionExtensions.cs#L143
namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Extension methods for reflection-related types.
/// </summary>
internal static class ReflectionExtensions
{
    /// <summary>
    /// Checks if a provided member has a <c>RequiredMemberAttribute</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On NET7+ this would <em>typically</em> be the framework supplied <c>RequiredMemberAttribute</c>, <em>but</em> internally the compiler
    /// <em>only</em> requires an attribute with that specific type <em>name</em>, not that specific type <em>reference</em>.
    /// </para>
    /// <para>
    /// This could very well be an internally defined custom polyfill attribute using that type name, so this
    /// check is done <em>only</em> via type <em>name</em>, not reference.
    /// </para>
    /// </remarks>
    /// <param name="memberInfo">Member to check.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="memberInfo"/> carries a <see cref="MemberInfo.CustomAttributes">CustomAttributeData</see> with
    /// a type <em>name</em> of <c>System.Runtime.CompilerServices.RequiredAttribute</c>; <see langword="false" /> otherwise.
    /// </returns>
    public static bool HasRequiredMemberAttribute(
        this MemberInfo memberInfo)
    {
        return memberInfo.CustomAttributes.Any(
            cad => cad.AttributeType.FullName == "System.Runtime.CompilerServices.RequiredMemberAttribute");
    }
}
