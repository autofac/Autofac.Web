using System;

namespace Autofac.Integration.Web.Forms
{
    /// <summary>
    /// Used to hold IContainerProviderAccessor and its namespace Regex pattern constraint
    /// </summary>
    public class ContainerProviderAccessorConstraint
    {
        readonly IContainerProviderAccessor _providerAccessor;
        readonly string _namespaceRegexPattern;

        /// <summary>
        /// Class contstructor
        /// </summary>
        /// <param name="providerAccessor">required</param>
        /// <param name="namespaceRegexPattern">may be null</param>
        public ContainerProviderAccessorConstraint(IContainerProviderAccessor providerAccessor, string namespaceRegexPattern)
        {
            if (providerAccessor == null) throw new ArgumentNullException("providerAccessor");
            _providerAccessor = providerAccessor;
            _namespaceRegexPattern = namespaceRegexPattern;
        }


        /// <summary>
        /// ContainerProviderAccessor
        /// </summary>
        public IContainerProviderAccessor ProviderAccessor { get { return _providerAccessor; } }

        /// <summary>
        /// Namespace Regex pattern constraint
        /// </summary>
        public string NamespaceRegexPattern { get { return _namespaceRegexPattern; } }
    }
}
