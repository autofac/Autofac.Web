// Contributed by Alexander Dorofeev 2016-09-29
// Copyright © 2016 Autofac Contributors
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.


using System.Configuration;

namespace Autofac.Integration.Web.Configuration
{
    /// <summary>
    /// Used to specify module that implements IContainerProviderAccessor and constrain injection namespaces 
    /// </summary>
    public class ContainerProviderModuleElement :ConfigurationElement
    {
        const string NAME_PROP_NAME = "Name";
        const string INJECTION_NAMESPACE_REGEX_PROP_NAME = "InjectionNamespaceRegex";

        /// <summary>
        /// Name of http module that implements IContainerProviderAccessor 
        /// </summary>
        [ConfigurationProperty(NAME_PROP_NAME, IsRequired = true)]
        public string Name
        {
            get
            {
                return base[NAME_PROP_NAME] as string;
            }
            set
            {
                base[NAME_PROP_NAME] = value;
            }
        }

        /// <summary>
        /// Regex for constraining injection namespaces 
        /// </summary>
        [ConfigurationProperty(INJECTION_NAMESPACE_REGEX_PROP_NAME)]
        public string InjectionNamespaceRegex
        {
            get
            {
                return base[INJECTION_NAMESPACE_REGEX_PROP_NAME] as string;
            }
            set
            {
                base[INJECTION_NAMESPACE_REGEX_PROP_NAME] = value;
            }
        }
    }
}
