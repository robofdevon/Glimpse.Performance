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
        protected HttpContext HttpContextState;

        public HttpContextStorageProvider() : this(HttpContext.Current)
        {

        }

        public HttpContextStorageProvider(HttpContext httpContext)
        {
            HttpContextState = httpContext;
            Initialise();
        }

        public void Store(MethodInvocation methodInvocation)
        {
            var storage = GetStorageCollection();
            if (storage != null)
            {
                storage.Push(methodInvocation);
            }
        }

        public IEnumerable<MethodInvocation> Retrieve()
        {
            return GetStorageCollection();
        }

        public void Clear()
        {
            var storage = GetStorageCollection();
            if (storage != null)
            {
                storage.Clear();
            }
        }

        protected virtual bool HttpContextIsInitialised()
        {
            return HttpContextState != null;
        }

        protected void Initialise() 
        {
            if (HttpContextIsInitialised())
            {
                InitialiseStorageCollection();
            }
        }

        protected void InitialiseStorageCollection()
        {
            lock (HttpContextState.Items)
            {
                if (!HttpContextState.Items.Contains(StorageKey))
                {
                    HttpContextState.Items.Add(StorageKey, new Stack<MethodInvocation>());
                }
            }
        }

        protected virtual Stack<MethodInvocation> GetStorageCollection()
        {
            if (HttpContextIsInitialised())
            {
                return HttpContextState.Items[StorageKey] as Stack<MethodInvocation>;
            }

            return null;
        }
    }
}
