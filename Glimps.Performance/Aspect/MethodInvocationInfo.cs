using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Performance.Aspect
{
    public class MethodInvocationInfo
    {
        public string Class { get; set; }
        public string Method { get; set; }
        public string Params { get; set; }
        public long InvocationTimeMilliseconds { get; set; }
    }
}
