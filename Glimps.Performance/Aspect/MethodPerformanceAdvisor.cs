using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PostSharp.Aspects;
using System.Reflection;
using System.Web;
using Glimpse.Performance.Interface;
using Glimpse.Performance.Factory;
using Glimpse.Performance.Config;

namespace Glimpse.Performance.Aspect
{
    [Serializable]
    public class MethodPerformanceAdvisor : BasePerformanceAdvisor
    {
        static readonly Stopwatch stopwatch = new Stopwatch();
        
        //cache per request lifetime
        private bool enabled = false;
        private bool enabledSet = false;
        private static object syncRoot = new object();

        static MethodPerformanceAdvisor()
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
            if(! enabledSet)
            {
                lock(syncRoot)
                {
                    if(! enabledSet)
                    {
                        enabledSet = true;
                        enabled = GlimpsePerformanceConfiguration.Instance.Enabled;
                    }
                } 
            }
            
            //don't log is disabled
            //don't log properties, this would be too verbose
            if (enabled && !isProperty(args))
            {
                args.MethodExecutionTag = stopwatch.ElapsedMilliseconds;
            }
        }

        //Cache for request lifetime
        private IStorageProvider storageProvider;

        /// <summary> 
        /// Method invoked after the execution of the method to which the current 
        /// aspect is applied. 
        /// </summary> 
        /// <param name="args">Unused.</param> 
        public override void OnExit(MethodExecutionArgs args)
        {
            //Don't log if not in a web request, relying on state
            //don't log properties, this would be too verbose
            if (enabled && !isProperty(args))
            {
                var methodInfo = new MethodInvocationInfo
                {
                    Class = args.Method.DeclaringType.FullName,
                    Method = args.Method.Name,
                    Params = string.Join<ParameterInfo>(", ", args.Method.GetParameters()),
                    InvocationTimeMilliseconds = stopwatch.ElapsedMilliseconds - (long)args.MethodExecutionTag
                };

                if(storageProvider == null)
                {
                    storageProvider = StorageFactory.GetStorageProvider();
                } 

                storageProvider.Store(methodInfo);
            }
        }
    }
}
