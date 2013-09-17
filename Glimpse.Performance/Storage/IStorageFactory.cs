// -----------------------------------------------------------------------
// <copyright file="IStorageFactory.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Storage
{
    public interface IStorageFactory
    {
        IStorageProvider Get();
    }
}
