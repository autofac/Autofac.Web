using Autofac.Integration.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Base class for module implementations
    /// </summary>
    public abstract class ModuleBase
    {
        /// <summary>
        /// Current appllication
        /// </summary>
        protected HttpApplication httpApplication;

        /// <summary>
        /// ContainerProviderAccessor's and namespace prefixes applicable to them
        /// </summary>
        protected IEnumerable<ContainerProviderAccessorConstraint> containerProviderAccessors;
        const string SECTION_NAME = "Autofac.Integration.Web";

        /// <summary>
        /// Populates ContainerProviderAccessor's and namespace prefixes from application
        /// </summary>
        /// <param name="context"></param>
        protected void AddContainerProviderAccessors(HttpApplication context)
        {
            var accesssors = new List<ContainerProviderAccessorConstraint>();
            var section = ConfigurationManager.GetSection(SECTION_NAME) as AutofacWebFormsIntegrationSection;
            if (section != null)
            {
                AddModuleContainerProviderAccessors(context, accesssors, section);
            }
            else
            {
                AddApplicationContainerProviderAccessor(context, accesssors);
            }

            containerProviderAccessors = accesssors;
        }

        private static void AddModuleContainerProviderAccessors(HttpApplication context, List<ContainerProviderAccessorConstraint> accesssors, AutofacWebFormsIntegrationSection section)
        {
            foreach (var moduleElement in section.Modules)
            {
                var httpModule = context.Modules[moduleElement.Name];
                if (httpModule == null)
                    throw new InvalidOperationException(string.Format(DependencyInjectionModuleResources.HttpModuleMustExitInApplication,
                        moduleElement.Name));

                var containerProviderAccessor = httpModule as IContainerProviderAccessor;
                if (containerProviderAccessor == null)
                    throw new InvalidOperationException(string.Format(DependencyInjectionModuleResources.ModuleMustImplementAccessor,
                        moduleElement.Name));
                if (string.IsNullOrEmpty(moduleElement.InjectionNamespaceRegex) && accesssors.Any())
                    throw new InvalidOperationException(DependencyInjectionModuleResources.RegexMustBeSpecified);

                if (accesssors.Any(c => c.NamespaceRegexPattern == moduleElement.InjectionNamespaceRegex))
                    throw new InvalidOperationException(DependencyInjectionModuleResources.RegexMustBeUnique);

                accesssors.Add(new ContainerProviderAccessorConstraint(containerProviderAccessor, moduleElement.InjectionNamespaceRegex));

            }
        }

        private void AddApplicationContainerProviderAccessor(HttpApplication context, List<ContainerProviderAccessorConstraint> constraints)
        {

            var containerProviderAccessor = context as IContainerProviderAccessor;

            if (containerProviderAccessor == null)
                throw new InvalidOperationException(DependencyInjectionModuleResources.ApplicationMustImplementAccessor);
            constraints.Add(new ContainerProviderAccessorConstraint(containerProviderAccessor, null));

        }
    }
}
