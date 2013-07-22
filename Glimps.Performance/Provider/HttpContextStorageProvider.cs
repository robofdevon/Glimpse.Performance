using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Performance.Interface;
using System.Web;
using Glimpse.Performance.Aspect;

namespace Glimpse.Performance.Provider
{
    public class HttpContextStorageProvider : IStorageProvider
    {
        private const string storageKey = "_methodInfo";
        public void Store(MethodInvocationInfo methodInfo)
        {
            init();
            ((Stack<MethodInvocationInfo>)HttpContext.Current.Items[storageKey]).Push(methodInfo);
        }

        public IEnumerable<MethodInvocationInfo> Retreive()
        {
            return HttpContext.Current.Items[storageKey] as Stack<MethodInvocationInfo>;
        }

        private void init() 
        {
            lock (HttpContext.Current.Items)
            {
                if (!HttpContext.Current.Items.Contains(storageKey))
                {
                    HttpContext.Current.Items.Add(storageKey, new Stack<MethodInvocationInfo>());
                }
            }
        }
    }
}
