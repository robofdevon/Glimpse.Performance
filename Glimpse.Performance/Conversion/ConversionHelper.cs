// -----------------------------------------------------------------------
// <copyright file="ConversionHelper.cs" company="">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Conversion
{
    using Aspect;
    using PostSharp.Aspects;

    public class ConversionHelper : IConversionHelper
    {
        public MethodInvocation ToMethodInvocation(MethodExecutionArgs methodExecutionArgs, long invocationTimeMilliseconds)
        {
            return new MethodInvocation
            {
                ClassName = methodExecutionArgs.GetClassName(),
                MethodName = methodExecutionArgs.GetMethodName(),
                Parameters = methodExecutionArgs.GetParameterTypesAndNamesCsv(),
                InvocationTimeMilliseconds = invocationTimeMilliseconds
            };
        }
    }
}
