//-----------------------------------------------------------------------
// <copyright file="MethodPerformanceAdvisor.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using Config;
    using PostSharp.Aspects;
    using System;
    using System.Reflection;
    using Time;
    using Storage;
    
    [Serializable]
    public class MethodPerformanceAdvisor : BaseAdvisor
    {
        //todo: look at refactoring this class, cut the responsibilities up
        private static IStopwatch _stopWatch;
        private static readonly object SyncRoot = new object();
        private readonly IGlimpsePerformanceConfiguration _glimpsePerformanceConfiguration;
        private bool _enabled;
        private bool _isInitialised;
        private IStorageProvider _storageProvider;

        static MethodPerformanceAdvisor()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public MethodPerformanceAdvisor()
        {
            _glimpsePerformanceConfiguration = new GlimpsePerformanceConfiguration();
        }

        public MethodPerformanceAdvisor(IStopwatch stopWatch, 
            IGlimpsePerformanceConfiguration glimpsePerformanceConfiguration,
            IStorageProvider storageProvider)
        {
            _glimpsePerformanceConfiguration = glimpsePerformanceConfiguration;
            _stopWatch = stopWatch;
            _storageProvider = storageProvider;
            _stopWatch.Start();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            MethodExecutionArguments = args;
            Initialise();
            SetStartTime();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            MethodExecutionArguments = args;
            ProcessMethodExit();
        }

        protected virtual void Initialise()
        {
            if(!_isInitialised)
            {
                lock (SyncRoot)
                {
                    InitialiseSettings();
                    InitialiseStopwatch();
                    _isInitialised = true;
                }
            }
        }

        protected virtual void SetStartTime()
        {
            if (_enabled && !IsProperty())
            {
                MethodExecutionArguments.MethodExecutionTag = _stopWatch.ElapsedMilliseconds;
            }
        }

        protected virtual void InitialiseSettings()
        {
            _enabled = _glimpsePerformanceConfiguration.Enabled;
        }

        protected virtual void InitialiseStopwatch()
        {
            if (_enabled && !IsProperty())
            {
                MethodExecutionArguments.MethodExecutionTag = _stopWatch.ElapsedMilliseconds;
            }
        }

        protected virtual void ProcessMethodExit()
        {
            if (_enabled && !IsProperty())
            {
                var invacationTimeMilliseconds = GetInvocationTimeMilliseconds();
                var methodInvocation = GetMethodInvocation(invacationTimeMilliseconds);
                StoreMethodInvocation(methodInvocation);
            }
        }

        protected virtual string GetMethodName()
        {
            string methodName = null;
            if (MethodExecutionArguments != null
                && MethodExecutionArguments.Method != null)
            {
                methodName = MethodExecutionArguments.Method.Name;
            }

            return methodName;
        }

        protected virtual string GetClassName()
        {
            string className = null;
            if (MethodExecutionArguments != null
                && MethodExecutionArguments.Method != null
                && MethodExecutionArguments.Method.DeclaringType != null)
            {
                className = MethodExecutionArguments.Method.DeclaringType.FullName;
            }

            return className;
        }

        protected virtual string GetParameters()
        {
            if (MethodExecutionArguments == null
                || MethodExecutionArguments.Method == null)
            {
                return "";
            }

            return string.Join<ParameterInfo>(", ", MethodExecutionArguments.Method.GetParameters());
        }

        protected virtual MethodInvocation GetMethodInvocation(long invocationTimeMilliseconds)
        {
            return new MethodInvocation
            {
                ClassName = GetClassName(),
                MethodName = GetMethodName(),
                Parameters = GetParameters(),
                InvocationTimeMilliseconds = invocationTimeMilliseconds
            };
        }

        protected virtual long GetInvocationTimeMilliseconds()
        {
            var stopTime = _stopWatch.ElapsedMilliseconds;
            var startTime = (long)MethodExecutionArguments.MethodExecutionTag;
            return stopTime - startTime;
        }

        protected virtual void StoreMethodInvocation(MethodInvocation methodInvocation)
        {
            if (_storageProvider == null)
            {
                _storageProvider = GetStorageFactory().Get();
            }

            _storageProvider.Store(methodInvocation);
        }

        //todo: dependency inversion
        protected virtual IStorageFactory GetStorageFactory()
        {
            return new StorageFactory();
        }
    }
}
