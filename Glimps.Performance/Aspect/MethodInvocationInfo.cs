//-----------------------------------------------------------------------
// <copyright file="MethodInvocationInfo.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Aspect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a single method call.
    /// </summary>
    public class MethodInvocationInfo
    {
        /// <summary>
        /// Gets or sets the class of the subject method.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the name of the subject method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list of the subject method's parameters, including the types - i.e. "string s, long l"
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Gets or sets the invocation time of the subject method, including any children.
        /// </summary>
        public long InvocationTimeMilliseconds { get; set; }
    }
}
