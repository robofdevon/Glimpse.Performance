using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Performance.Aspect
{
    public class MethodInvocationInfo
    {
        public string MethodName { get; set; }
        public long InvocationTimeMilliseconds { get; set; }
    }
}
