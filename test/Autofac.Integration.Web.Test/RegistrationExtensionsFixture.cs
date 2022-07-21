// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace Autofac.Integration.Web.Test
{
    public class RegistrationExtensionsFixture
    {
        private interface ICounter
        {
        }

        private class Counter : ICounter
        {
            public static int Constructed { get; private set; }

            public Counter()
            {
                Constructed++;
            }
        }

        public static HttpContext FakeHttpContext()
        {
            // source: https://stackoverflow.com/a/10126711/6887257
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    CallingConventions.Standard,
                    new[] { typeof(HttpSessionStateContainer) },
                    null)
                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        [Fact]
        public void VerifyInstanceLifetimeIsSession()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Counter>().AsSelf().As<ICounter>().CacheInSession();
            var container = builder.Build();

            HttpContext.Current = FakeHttpContext();
            using (var scope = container.BeginLifetimeScope())
            {
                var iCounter = scope.Resolve<ICounter>();
                Assert.IsType<Counter>(iCounter);
                Assert.Equal(1, Counter.Constructed);

                scope.Resolve<Counter>();
                Assert.Equal(1, Counter.Constructed);
            }

            HttpContext.Current = FakeHttpContext();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<Counter>();
                Assert.Equal(2, Counter.Constructed);
            }
        }
    }
}
