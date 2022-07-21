// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Web;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;

namespace Autofac.Integration.Web
{
    /// <summary>
    /// Extends registration syntax for common web scenarios.
    /// </summary>
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Share one instance of the component within the context of a single
        /// HTTP request.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="registration">The registration to configure.</param>
        /// <param name="lifetimeScopeTags">Additional tags applied for matching lifetime scopes.</param>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="registration" /> is <see langword="null" />.
        /// </exception>
        [Obsolete("Instead of using the web-forms-specific InstancePerHttpRequest, please switch to the InstancePerRequest shared registration extension from Autofac core.")]
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InstancePerHttpRequest<TLimit, TActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration, params object[] lifetimeScopeTags)
        {
            return registration.InstancePerRequest(lifetimeScopeTags);
        }

        /// <summary>
        /// Cache instances in the web session. This implies external ownership (disposal is not
        /// available.) All dependencies must also have external ownership.
        /// </summary>
        /// <remarks>
        /// It is strongly recommended that components cached per-session do not take dependencies on
        /// other services.
        /// </remarks>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">The registration to configure.</param>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is the responsibility of the registry to dispose of registrations.")]
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle>
            CacheInSession<TLimit, TActivatorData, TSingleRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
            where TActivatorData : IConcreteActivatorData
            where TSingleRegistrationStyle : SingleRegistrationStyle
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            var services = registration.RegistrationData.Services.ToArray();
            registration.RegistrationData.ClearServices();

            return registration
                .ExternallyOwned()
                .OnRegistered(e =>
                {
                    foreach (var service in services)
                    {
                        e.ComponentRegistryBuilder.Register(RegistrationBuilder
                            .ForDelegate((c, p) =>
                            {
                                var session = HttpContext.Current.Session;
                                object result;
                                lock (session.SyncRoot)
                                {
                                    result = session[e.ComponentRegistration.Id.ToString()];
                                    if (result == null)
                                    {
                                        var resolveRequest = new ResolveRequest(service, new ServiceRegistration(ServicePipelines.DefaultServicePipeline, e.ComponentRegistration), p);
                                        result = c.ResolveComponent(resolveRequest);
                                        session[e.ComponentRegistration.Id.ToString()] = result;
                                    }
                                }

                                return result;
                            })
                            .As(service)
                            .InstancePerLifetimeScope()
                            .ExternallyOwned()
                            .CreateRegistration());
                    }
                });
        }
    }
}
