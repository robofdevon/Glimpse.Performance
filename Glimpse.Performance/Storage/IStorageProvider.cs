//-----------------------------------------------------------------------
// <copyright file="IStorageProvider.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Storage
{
    using Aspect;
    using System.Collections.Generic;
    
    public interface IStorageProvider
    {
        void Store(MethodInvocation methodInvocation);
        IEnumerable<MethodInvocation> Retrieve();
        void Clear();
    }
}
