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
            // Don't log properties, this would be too verbose
            if (this.enabled && !this.IsProperty(args))
            {
                var invacationTimeMilliseconds = this.GetInvocationTimeMilliseconds((long)args.MethodExecutionTag);
                var methodInfo = this.GetMethodInvocationInfo(args, invacationTimeMilliseconds);
                this.StoreMethodInvocationInfo(methodInfo);
            }
        }

        /// <summary>
        /// Gets the method name from the <see cref="MethodExecutionArgs"/>
        /// </summary>
        /// <param name="args">The <see cref="MethodExecutionArgs"/> to extract the method name from.</param>
        /// <returns>A method name.</returns>
        protected virtual string GetMethodName(MethodExecutionArgs args)
        {
            string methodName = null;
            if (args != null
                && args.Method != null)
            {
                methodName = args.Method.Name;
            }

            return methodName;
        }

        /// <summary>
        /// Gets the class name from the <see cref="MethodExecutionArgs"/>
        /// </summary>
        /// <param name="args">The <see cref="MethodExecutionArgs"/> to extract the class name from.</param>
        /// <returns>A class name.</returns>
        protected virtual string GetClassName(MethodExecutionArgs args)
        {
            string className = null;
            if (args != null
                && args.Method != null
                && args.Method.DeclaringType != null)
            {
                className = args.Method.DeclaringType.FullName;
            }

            return className;
        }

        /// <summary>
        /// Gets a comma delimited list of parameter names from the <see cref="MethodExecutionArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="MethodExecutionArgs"/> to extract the parameter names from.</param>
        /// <returns>A comma delimited list of parameter names.</returns>
        protected virtual string GetParameters(MethodExecutionArgs args)
        {
            return string.Join<ParameterInfo>(", ", args.Method.GetParameters());
        }

        /// <summary>
        /// Gets a <see cref="MethodInvocationInfo"/> from the specified <see cref="MethodExecutionArgs"/> and invocation time.
        /// </summary>
        /// <param name="args">The <see cref="MethodExecutionArgs"/>.</param>
        /// <param name="invocationTimeMilliseconds">The invocation time of the method in milliseconds.</param>
        /// <returns>A <see cref="MethodInvocationInfo"/> object representing the method invocation.</returns>
        protected virtual MethodInvocationInfo GetMethodInvocationInfo(MethodExecutionArgs args, long invocationTimeMilliseconds)
        {
            var methodInfo = new MethodInvocationInfo
            {
                Class = this.GetClassName(args),
                Method = this.GetMethodName(args),
                Params = this.GetParameters(args),
                InvocationTimeMilliseconds = invocationTimeMilliseconds
            };

            return methodInfo;
        }

        /// <summary>
        /// Gets the method invocation time from the specified time offset.
        /// </summary>
        /// <param name="startTimeOffset">The start time offset.</param>
        /// <returns>The method invocation time in milliseconds.</returns>
        protected virtual long GetInvocationTimeMilliseconds(long startTimeOffset)
        {
            return Stopwatch.ElapsedMilliseconds - startTimeOffset;
        }

        /// <summary>
        /// Stores a <see cref="MethodInvocationInfo"/> using the configured storage provider.
        /// </summary>
        /// <param name="methodInvocationInfo">The method invocation information to store.</param>
        protected virtual void StoreMethodInvocationInfo(MethodInvocationInfo methodInvocationInfo)
        {
            if (this.storageProvider == null)
            {
                this.storageProvider = StorageFactory.GetStorageProvider();
            }

            this.storageProvider.Store(methodInvocationInfo);
        }
    }
}
