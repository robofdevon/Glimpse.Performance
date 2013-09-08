//-----------------------------------------------------------------------
// <copyright file="HttpContextStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Provider
{
    using Aspect;
    using Interface;
    using System.Collections.Generic;
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
            this.Initialise();
            var storage = this.GetStorageCollection();
            if (storage != null)
            {
                storage.Push(methodInfo);
            }
        }

        /// <summary>
        /// Retrieves the stored collection of <see cref="MethodInvocationInfo"/>.
        /// </summary>
        /// <returns>The stored collection of <see cref="MethodInvocationInfo"/></returns>
        public IEnumerable<MethodInvocationInfo> Retrieve()
        {
            return this.GetStorageCollection();
        }

        /// <summary>
        /// Clears any <see cref="MethodInvocationInfo"/> currently stored.
        /// </summary>
        public void Clear()
        {
            var storage = this.GetStorageCollection();
            if (storage != null)
            {
                storage.Clear();
            }
        }

        /// <summary>
        /// Returns whether HttpContext state has been initialized.
        /// </summary>
        /// <returns><c>true</c> is HttpContext state has been initialized, false otherwise.</returns>
        protected virtual bool HttpContextIsInitialised()
        {
            return HttpContext.Current != null;
        }

        /// <summary>
        /// Initializes the storage provider.
        /// </summary>
        protected virtual void Initialise() 
        {
            if (this.HttpContextIsInitialised())
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

        /// <summary>
        /// Gets the storage collection instance.
        /// </summary>
        /// <returns>The storage collection instance.</returns>
        protected virtual Stack<MethodInvocationInfo> GetStorageCollection()
        {
            if (this.HttpContextIsInitialised())
            {
                return HttpContext.Current.Items[StorageKey] as Stack<MethodInvocationInfo>;
            }

            return null;
        }
    }
}
