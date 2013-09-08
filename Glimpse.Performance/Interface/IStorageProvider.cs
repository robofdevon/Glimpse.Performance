//-----------------------------------------------------------------------
// <copyright file="IStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Interface
{
    using Glimpse.Performance.Aspect;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    /// <summary>
    /// An interface for abstracting how method info is stored and retrieved.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// Store the instance of <see cref="MethodInvocationInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The instance of <see cref="MethodInvocationInfo"/> to store.</param>
        void Store(MethodInvocationInfo methodInfo);

        /// <summary>
        /// Retrieve a collection of stored <see cref="MethodInvocationInfo"/>.
        /// </summary>
        /// <returns>A collection of stored <see cref="MethodInvocationInfo"/>.</returns>
        IEnumerable<MethodInvocationInfo> Retrieve();

        /// <summary>
        /// Clears any <see cref="MethodInvocationInfo"/> currently stored.
        /// </summary>
        void Clear();
    }
}
