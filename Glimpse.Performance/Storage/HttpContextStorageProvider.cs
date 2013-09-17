//-----------------------------------------------------------------------
// <copyright file="HttpContextStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Storage
{
    using Aspect;
    using System.Collections.Generic;
    using System.Web;
    
    //todo: unit tests
    public class HttpContextStorageProvider : IStorageProvider
    {
        private const string StorageKey = "_methodInfo";

        public void Store(MethodInvocation methodInvocation)
        {
            this.Initialise();
            var storage = this.GetStorageCollection();
            if (storage != null)
            {
                storage.Push(methodInvocation);
            }
        }

        public IEnumerable<MethodInvocation> Retrieve()
        {
            return this.GetStorageCollection();
        }

        public void Clear()
        {
            var storage = this.GetStorageCollection();
            if (storage != null)
            {
                storage.Clear();
            }
        }

        protected virtual bool HttpContextIsInitialised()
        {
            return HttpContext.Current != null;
        }

        protected virtual void Initialise() 
        {
            if (this.HttpContextIsInitialised())
            {
                lock (HttpContext.Current.Items)
                {
                    if (!HttpContext.Current.Items.Contains(StorageKey))
                    {
                        HttpContext.Current.Items.Add(StorageKey, new Stack<MethodInvocation>());
                    }
                }
            }
        }

        protected virtual Stack<MethodInvocation> GetStorageCollection()
        {
            if (this.HttpContextIsInitialised())
            {
                return HttpContext.Current.Items[StorageKey] as Stack<MethodInvocation>;
            }

            return null;
        }
    }
}
