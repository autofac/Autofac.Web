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

using System.Collections.Generic;
using System.Configuration;

namespace Autofac.Integration.Web.Configuration
{
    /// <summary>
    /// Collection for hosting module configuration elements
    /// </summary>
    [ConfigurationCollection(typeof(ContainerProviderModuleElement), AddItemName = MODULE)]
    public class ContainerProviderModuleElementCollection: ConfigurationElementCollection
    {
        private const string MODULE = "module";

        /// <summary>
        /// Creates new element
        /// </summary>
        /// <returns>New ContainerProviderModuleElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ContainerProviderModuleElement();
        }

        /// <summary>
        /// Collection indexer
        /// </summary>
        /// <param name="index"></param>
        /// <returns>ContainerProviderModuleElement</returns>
        public ContainerProviderModuleElement this[int index]
        {
            get
            {
                return BaseGet(index) as ContainerProviderModuleElement;
            }
        }

        /// <summary>
        /// Returns element key for collection
        /// </summary>
        /// <param name="element"></param>
        /// <returns>ContainerProviderModuleElement.Name</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var moduleElement = element as ContainerProviderModuleElement;
            if (moduleElement != null)
                return moduleElement.Name;
            else
                return null;
        }

        /// <summary>
        /// Collection enumerator
        /// </summary>
        /// <returns>IEnumerator of ContainerProviderModuleElement</returns>
        public new IEnumerator<ContainerProviderModuleElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ContainerProviderModuleElement;
            }
        }

    }
}
