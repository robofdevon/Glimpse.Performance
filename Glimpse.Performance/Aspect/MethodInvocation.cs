//-----------------------------------------------------------------------
// <copyright file="MethodInvocation.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using System;

    [Serializable]
    public class MethodInvocation
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public long InvocationTimeMilliseconds { get; set; }
    }
}
