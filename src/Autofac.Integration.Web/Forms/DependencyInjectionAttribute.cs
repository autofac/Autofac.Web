// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Base class for dependency injection attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class DependencyInjectionAttribute : Attribute
    {
    }
}
