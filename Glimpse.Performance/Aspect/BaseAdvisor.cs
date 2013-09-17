//-----------------------------------------------------------------------
// <copyright file="BaseAdvisor.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using PostSharp.Aspects;
    using System;
    using System.Reflection;
    
    [Serializable]
    public abstract class BaseAdvisor : OnMethodBoundaryAspect
    {
        [NonSerialized] 
        protected MethodExecutionArgs MethodExecutionArguments;

        protected bool IsProperty()
        {
            return MethodExecutionArguments.Method.IsSpecialName &&
                (MethodExecutionArguments.Method.Attributes & MethodAttributes.HideBySig) != 0;
        }
    }
}
