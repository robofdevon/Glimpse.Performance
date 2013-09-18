//-----------------------------------------------------------------------
// <copyright file="MethodPerformanceAdvisor.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using Config;
    using System;
    using Time;
    using Storage;
    using Conversion;
    
    [Serializable]
    public class MethodPerformanceAdvisor : BaseAdvisor
    {
        protected static IStopwatch Stopwatch;

        static MethodPerformanceAdvisor()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public MethodPerformanceAdvisor()
        {
            GlimpsePerformanceConfiguration = new GlimpsePerformanceConfiguration();
            ConversionHelper = new ConversionHelper();
            StorageFactory = new StorageFactory();
        }

        public MethodPerformanceAdvisor(IStopwatch stopwatch, 
            IGlimpsePerformanceConfiguration glimpsePerformanceConfiguration,
            IStorageFactory storageFactory,
            IConversionHelper conversionHelper)
        {
            GlimpsePerformanceConfiguration = glimpsePerformanceConfiguration;
            Stopwatch = stopwatch;
            StorageFactory = storageFactory;
            Stopwatch.Start();
            ConversionHelper = conversionHelper;
        }

        protected override void Initialise()
        {
            if(!IsInitialised)
            {
                lock (SyncRoot)
                {
                    InitialiseSettings();
                    InitialiseStartTime();
                    IsInitialised = true;
                }
            }
        }

        protected virtual void InitialiseStartTime()
        {
            if (DoMonitoring())
            {
                MethodExecutionArguments.MethodExecutionTag = Stopwatch.ElapsedMilliseconds;
            }
        }

        protected override void Finalise()
        {
            if (DoMonitoring())
            {
                var invacationTimeMilliseconds = GetInvocationTimeMilliseconds();
                var methodInvocation = ConversionHelper.ToMethodInvocation(MethodExecutionArguments, invacationTimeMilliseconds);
                StoreMethodInvocation(methodInvocation);
            }
        }

        protected virtual bool DoMonitoring()
        {
            return Enabled && !IsProperty();
        }

        protected virtual long GetInvocationTimeMilliseconds()
        {
            var stopTime = Stopwatch.ElapsedMilliseconds;
            var startTime = (long)MethodExecutionArguments.MethodExecutionTag;
            return stopTime - startTime;
        }

        protected virtual void StoreMethodInvocation(MethodInvocation methodInvocation)
        {
            if (StorageProvider == null)
            {
                StorageProvider = StorageFactory.Get();
            }

            StorageProvider.Store(methodInvocation);
        }
    }
}