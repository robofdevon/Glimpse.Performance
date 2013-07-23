//-----------------------------------------------------------------------
// <copyright file="BaseAdvisor.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using PostSharp.Aspects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    
    /// <summary>
    /// Base class for advisors.
    /// </summary>
    [Serializable]
    public abstract class BaseAdvisor : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Gets the Boolean value indicating whether the indicated MethodExecutionArgs relate to a property.
        /// </summary>
        /// <param name="args">The MethodExecutionArgs to test.</param>
        /// <returns>True if the value is a property; otherwise, false.</returns>
        protected bool IsProperty(MethodExecutionArgs args)
        {
            return args.Method.IsSpecialName &&
                (args.Method.Attributes & MethodAttributes.HideBySig) != 0;
        }
    }
}
