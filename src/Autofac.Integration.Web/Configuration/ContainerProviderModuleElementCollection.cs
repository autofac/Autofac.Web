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

using System;
using System.Collections.Generic;
using System.Configuration;

namespace Autofac.Integration.Web.Configuration
{
    /// <summary>
    /// Collection for hosting module configuration elements
    /// </summary>
    [ConfigurationCollection(typeof(ContainerProviderModuleElement), AddItemName = MODULE)]
    public sealed class ContainerProviderModuleElementCollection: ConfigurationElementCollection,ICollection<ContainerProviderModuleElement>
    {
        private const string MODULE = "module";

        bool ICollection<ContainerProviderModuleElement>.IsReadOnly
        {
            get
            {
                return IsReadOnly();
            }
        }

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

        /// <summary>
        /// Adds element to collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(ContainerProviderModuleElement item)
        {
            BaseAdd(item);
        }

        /// <summary>
        /// Clears collection
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Inicated whether collection contains element 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ContainerProviderModuleElement item)
        {
            return BaseIndexOf(item) != -1;
        }

        /// <summary>
        ///  CopyTo copies a collection into an Array, starting at a particular
        /// index into the array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(ContainerProviderModuleElement[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        
        /// <summary>
        /// Removes element from collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns>If item was in collection</returns>
        public bool Remove(ContainerProviderModuleElement item)
        {
           
            if (BaseIndexOf(item)!=-1)
            {
                BaseRemove(item);
                return true;
            }
            return false;
        }
    }
}
