using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PostSharp.Aspects;

namespace Glimpse.Performance.Aspect
{
    [Serializable]
    public class MethodPerformanceAttribute : BasePerformanceAttribute
    {
        static readonly Stopwatch stopwatch = new Stopwatch();

        static MethodPerformanceAttribute()
        {
            stopwatch.Start();
        }

        /// <summary> 
        /// Method invoked before the execution of the method to which the current 
        /// aspect is applied. 
        /// </summary> 
        /// <param name="args">Unused.</param> 
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = stopwatch.ElapsedMilliseconds;
            base.OnEntry(args);
        }

        /// <summary> 
        /// Method invoked after the execution of the method to which the current 
        /// aspect is applied. 
        /// </summary> 
        /// <param name="args">Unused.</param> 
        public override void OnExit(MethodExecutionArgs args)
        {
            long timeMilliseconds = stopwatch.ElapsedMilliseconds - (long)args.MethodExecutionTag;
            var methodInfo = new MethodInvocationInfo
                {
                    MethodName = args.Method.Name,
                    InvocationTimeMilliseconds = timeMilliseconds
                };

            this.MethodInvocationList.Push(methodInfo);
            Console.WriteLine(string.Format("{0} ({1} milliseconds)", methodInfo.MethodName, methodInfo.InvocationTimeMilliseconds));
            base.OnExit(args);
        }
    }
}
