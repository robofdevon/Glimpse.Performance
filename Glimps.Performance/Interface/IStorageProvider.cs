using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Performance.Aspect;

namespace Glimpse.Performance.Interface
{
    public interface IStorageProvider
    {
        void Store(MethodInvocationInfo methodInfo);
        IEnumerable<MethodInvocationInfo> Retreive();
    }
}
