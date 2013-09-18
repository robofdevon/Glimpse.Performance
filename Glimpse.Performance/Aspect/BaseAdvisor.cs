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
    using Storage;
    using Config;
    using Conversion;
    
    [Serializable]
    public abstract class BaseAdvisor : OnMethodBoundaryAspect
    {
        [NonSerialized] 
        protected MethodExecutionArgs MethodExecutionArguments;
        protected bool Enabled;
        protected bool IsInitialised;
        protected IStorageProvider StorageProvider;
        protected IStorageFactory StorageFactory;
        protected IGlimpsePerformanceConfiguration GlimpsePerformanceConfiguration;
        protected IConversionHelper ConversionHelper;
        protected static readonly object SyncRoot = new object();

        protected bool IsProperty()
        {
            return MethodExecutionArguments.Method.IsSpecialName &&
                (MethodExecutionArguments.Method.Attributes & MethodAttributes.HideBySig) != 0;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            MethodExecutionArguments = args;
            Initialise();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            MethodExecutionArguments = args;
            Finalise();
        }

        protected virtual void Initialise()
        {
            if (!IsInitialised)
            {
                lock (SyncRoot)
                {
                    InitialiseSettings();
                    IsInitialised = true;
                }
            }
        }

        protected virtual void InitialiseSettings()
        {
            Enabled = GlimpsePerformanceConfiguration.Enabled;
        }

        protected virtual void Finalise()
        {

        }
    }
}