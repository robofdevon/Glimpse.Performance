//-----------------------------------------------------------------------
// <copyright file="IStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Storage
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
        /// Store the instance of <see cref="MethodInvocation"/>.
        /// </summary>
        /// <param name="methodInvocation">The instance of <see cref="MethodInvocation"/> to store.</param>
        void Store(MethodInvocation methodInvocation);

        /// <summary>
        /// Retrieve a collection of stored <see cref="MethodInvocation"/>.
        /// </summary>
        /// <returns>A collection of stored <see cref="MethodInvocation"/>.</returns>
        IEnumerable<MethodInvocation> Retrieve();

        /// <summary>
        /// Clears any <see cref="MethodInvocation"/> currently stored.
        /// </summary>
        void Clear();
    }
}
