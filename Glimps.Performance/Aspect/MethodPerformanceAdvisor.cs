//-----------------------------------------------------------------------
// <copyright file="MethodPerformanceAdvisor.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using Glimpse.Performance.Config;
    using Glimpse.Performance.Factory;
    using Glimpse.Performance.Interface;
    using PostSharp.Aspects;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web;
    
    /// <summary>
    /// An advisor which adds performance based method advice to a subject.
    /// </summary>
    [Serializable]
    public class MethodPerformanceAdvisor : BaseAdvisor
    {
        /// <summary>
        /// Used to gather timing information.
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// Object used for locking, to ensure thread safe access.
        /// </summary>
        private static object syncRoot = new object();
        
        // Cache variables per request lifetime
        
        /// <summary>
        /// Flags whether the advisor is enabled.
        /// </summary>
        private bool enabled = false;

        // Cache for request lifetime

        /// <summary>
        /// Holds a reference to the storage provider to be used.
        /// </summary>
        private IStorageProvider storageProvider;

        /// <summary>
        /// Flags whether the enabled flag has been set in this web request.
        /// </summary>
        private bool enabledSet = false;

        /// <summary>
        /// Initializes static members of the <see cref="MethodPerformanceAdvisor" /> class.
        /// Starts the stop watch running.
        /// </summary>
        static MethodPerformanceAdvisor()
        {
            Stopwatch.Start();
        }

        /// <summary> 
        /// Method invoked before the execution of the method to which the current 
        /// aspect is applied. If enabled = true and the context is not a property,
        /// store the start time in the args. This instance will be passed onto the OnExit call.
        /// </summary> 
        /// <param name="args">The MethodExecutionArgs passed in from the subject method.</param> 
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (!this.enabledSet)
            {
                lock (syncRoot)
                {
                    if (!this.enabledSet)
                    {
                        this.enabledSet = true;
                        this.enabled = GlimpsePerformanceConfiguration.Instance.Enabled;
                    }
                } 
            }
            
            // Don't log is disabled
            // Don't log properties, this would be too verbose
            if (this.enabled && !this.IsProperty(args))
            {
                args.MethodExecutionTag = Stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary> 
        /// Method invoked after the execution of the method to which the current 
        /// aspect is applied. If enabled = true and the context is not a property,
        /// capture the subject method info, including the execution time and store
        /// the information.
        /// </summary> 
        /// <param name="args">The MethodExecutionArgs passed in from the subject method.</param> 
        public override void OnExit(MethodExecutionArgs args)
        {
            // Don't log if not in a web request, relying on state
            // Don't log properties, this would be too verbose
            if (this.enabled && !this.IsProperty(args))
            {
                var methodInfo = new MethodInvocationInfo
                {
                    Class = args.Method.DeclaringType.FullName,
                    Method = args.Method.Name,
                    Params = string.Join<ParameterInfo>(", ", args.Method.GetParameters()),
                    InvocationTimeMilliseconds = Stopwatch.ElapsedMilliseconds - (long)args.MethodExecutionTag
                };

                if (this.storageProvider == null)
                {
                    this.storageProvider = StorageFactory.GetStorageProvider();
                } 

                this.storageProvider.Store(methodInfo);
            }
        }
    }
}
