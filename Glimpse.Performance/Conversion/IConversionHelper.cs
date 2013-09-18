// -----------------------------------------------------------------------
// <copyright file="IConversionHelper.cs" company="">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Conversion
{
    using Aspect;
    using PostSharp.Aspects;

    public interface IConversionHelper
    {
        MethodInvocation ToMethodInvocation(MethodExecutionArgs methodExecutionArgs, long invocationTimeMilliseconds);
    }
}
