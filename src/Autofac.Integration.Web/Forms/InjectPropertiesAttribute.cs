// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Web.Forms;

/// <summary>
/// Indicates that property injection should be performed on the instance when it is instantiated.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class InjectPropertiesAttribute : DependencyInjectionAttribute
{
}
