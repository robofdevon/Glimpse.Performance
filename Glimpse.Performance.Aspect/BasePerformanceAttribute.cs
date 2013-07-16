using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using System.Reflection;

namespace Glimpse.Performance.Aspect
{
    [Serializable]
    public class BasePerformanceAttribute : OnMethodBoundaryAspect
    {
        // Serialized fields: set at build time, used at run time. 
        private string instanceName;

        // Not serialized because used at rutime only. 
        [NonSerialized]
        private Stack<MethodInvocationInfo> methodInvocationList;

        // Not serialized because used at build time only. 
        [NonSerialized]
        private bool includeInstanceName;

        /// <summary> 
        /// Gets the performance counter (can be invoked at runtime). 
        /// </summary> 
        protected Stack<MethodInvocationInfo> MethodInvocationList
        {
            get { return this.methodInvocationList; }
        }

        /// <summary> 
        /// Determines whether the performance counter must include an instance name. 
        /// If <c>true</c>, the instance name is set to the full method name. 
        /// </summary> 
        public bool UseInstanceName
        {
            get { return this.includeInstanceName; }
            set { this.includeInstanceName = value; }
        }

        /// <summary> 
        /// Method executed at build time. Initializes the aspect instance. After the execution 
        /// of <see cref="CompileTimeInitialize"/>, the aspect is serialized as a managed  
        /// resource inside the transformed assembly, and deserialized at runtime. 
        /// </summary> 
        /// <param name="method">Method to which the current aspect instance  
        /// has been applied.</param> 
        /// <param name="aspectInfo">Unused.</param> 
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            if (this.includeInstanceName)
            {
                this.instanceName = method.DeclaringType.FullName + "." + method.Name;
            }
        }

        /// <summary> 
        /// Method executed at run time just after the aspect is deserialized. 
        /// </summary> 
        /// <param name="method">>Method to which the current aspect instance  
        /// has been applied.</param> 
        public override void RuntimeInitialize(MethodBase method)
        {
            this.methodInvocationList = new Stack<MethodInvocationInfo>();
        }
    }
}
