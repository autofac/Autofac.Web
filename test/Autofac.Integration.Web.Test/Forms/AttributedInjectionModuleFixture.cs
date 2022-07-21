// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Web;
using Autofac.Integration.Web.Forms;
using Xunit;

namespace Autofac.Integration.Web.Test.Forms
{
    public class AttributedInjectionModuleFixture
    {
        private const string ContextSuppliedString = "ContextSuppliedString";
        private const string ExplicitlyProvidedString = "ExplicitlyProvidedString";

        private class HttpHandler : IHttpHandler
        {
            public bool IsReusable
            {
                get { throw new NotImplementedException(); }
            }

            public void ProcessRequest(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }

        [InjectProperties]
        private class PropertyInjectedPage : HttpHandler
        {
            public string Property { get; set; }
        }

        [InjectUnsetProperties]
        private class UnsetPropertyInjectedPage : HttpHandler
        {
            public string Property { get; set; }
        }

        private class NonInjectedPage : HttpHandler
        {
            public string Property { get; set; }
        }

        [Fact]
        public void PropertyInjected()
        {
            var context = CreateContext();
            var page = new PropertyInjectedPage();
            var target = new AttributedInjectionModule();
            var injector = target.GetInjectionBehavior(page);
            Assert.NotNull(injector);
            injector.InjectDependencies(context, page);
            Assert.Equal(ContextSuppliedString, page.Property);
        }

        [Fact]
        public void PropertyInjectedValueSet()
        {
            var context = CreateContext();
            var page = new PropertyInjectedPage
            {
                Property = ExplicitlyProvidedString,
            };
            var target = new AttributedInjectionModule();
            var injector = target.GetInjectionBehavior(page);
            Assert.NotNull(injector);
            injector.InjectDependencies(context, page);
            Assert.Equal(ContextSuppliedString, page.Property);
        }

        [Fact]
        public void UnsetPropertyInjected()
        {
            var context = CreateContext();
            var page = new UnsetPropertyInjectedPage();
            var target = new AttributedInjectionModule();
            var injector = target.GetInjectionBehavior(page);
            Assert.NotNull(injector);
            injector.InjectDependencies(context, page);
            Assert.Equal(ContextSuppliedString, page.Property);
        }

        [Fact]
        public void PropertyNotInjectedWhenValueSet()
        {
            var context = CreateContext();
            var page = new UnsetPropertyInjectedPage
            {
                Property = ExplicitlyProvidedString,
            };
            var target = new AttributedInjectionModule();
            var injector = target.GetInjectionBehavior(page);
            Assert.NotNull(injector);
            injector.InjectDependencies(context, page);
            Assert.Equal(ExplicitlyProvidedString, page.Property);
        }

        [Fact]
        public void PropertyNotInjected()
        {
            var context = CreateContext();
            var page = new NonInjectedPage();
            var target = new AttributedInjectionModule();
            var injector = target.GetInjectionBehavior(page);
            Assert.NotNull(injector);
            injector.InjectDependencies(context, page);
            Assert.Null(page.Property);
        }

        private IComponentContext CreateContext()
        {
            var cb = new ContainerBuilder();
            cb.RegisterInstance(ContextSuppliedString);
            return cb.Build();
        }
    }
}
