//-----------------------------------------------------------------------
// <copyright file="HttpContextStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Provider
{
    using Glimpse.Performance.Aspect;
    using Glimpse.Performance.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    
    /// <summary>
    /// A storage provider based on the web request HttpContext state.
    /// </summary>
    public class HttpContextStorageProvider : IStorageProvider
    {
        /// <summary>
        /// A key to reference the storage data.
        /// </summary>
        private const string StorageKey = "_methodInfo";

        /// <summary>
        /// Stores the <see cref="MethodInvocationInfo"/> into HttpContext state.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInvocationInfo"/> to store into HttpContext state.</param>
        public void Store(MethodInvocationInfo methodInfo)
        {
            this.Init();
            ((Stack<MethodInvocationInfo>)HttpContext.Current.Items[StorageKey]).Push(methodInfo);
        }

        /// <summary>
        /// Retrieves the stored collection of <see cref="MethodInvocationInfo"/>.
        /// </summary>
        /// <returns>The stored collection of <see cref="MethodInvocationInfo"/></returns>
        public IEnumerable<MethodInvocationInfo> Retrieve()
        {
            return HttpContext.Current.Items[StorageKey] as Stack<MethodInvocationInfo>;
        }

        /// <summary>
        /// Initializes the storage collection.
        /// </summary>
        private void Init() 
        {
            lock (HttpContext.Current.Items)
            {
                if (!HttpContext.Current.Items.Contains(StorageKey))
                {
                    HttpContext.Current.Items.Add(StorageKey, new Stack<MethodInvocationInfo>());
                }
            }
        }
    }
}
