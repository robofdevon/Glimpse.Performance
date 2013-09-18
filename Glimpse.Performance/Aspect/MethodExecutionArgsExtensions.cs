// -----------------------------------------------------------------------
// <copyright file="MethodExecutionArgumentsExtensions.cs" company="">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using System.Reflection;
    using PostSharp.Aspects;

    public static class MethodExecutionArgsExtensions
    {
        public static string GetMethodName(this MethodExecutionArgs methodExecutionArgs)
        {
            string methodName = null;
            if (methodExecutionArgs != null
                && methodExecutionArgs.Method != null)
            {
                methodName = methodExecutionArgs.Method.Name;
            }
            return methodName;
        }

        public static string GetClassName(this MethodExecutionArgs methodExecutionArgs)
        {
            string className = null;
            if (methodExecutionArgs != null
                && methodExecutionArgs.Method != null
                && methodExecutionArgs.Method.DeclaringType != null)
            {
                className = methodExecutionArgs.Method.DeclaringType.FullName;
            }

            return className;
        }

        public static string GetParameterTypesAndNamesCsv(this MethodExecutionArgs methodExecutionArgs)
        {
            if (methodExecutionArgs == null
                || methodExecutionArgs.Method == null)
            {
                return "";
            }

            return string.Join<ParameterInfo>(", ", methodExecutionArgs.Method.GetParameters());
        }
    }
}
