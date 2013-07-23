//-----------------------------------------------------------------------
// <copyright file="StorageFactory.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Factory
{
    using Glimpse.Performance.Config;
    using Glimpse.Performance.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A storage factory, used to return an instance implementing the IStorageProvider interface.
    /// The type returned depends on configuration - GlimpsePerformanceConfiguration.Instance.StorageProvider.
    /// </summary>
    public static class StorageFactory
    {
        // Cache per web request

        /// <summary>
        /// A locally cached storage provider instance - cached per web request.
        /// </summary>
        private static IStorageProvider storageProvider;

        /// <summary>
        /// Factory method - returns an instance implementing the IStorageProvider interface.
        /// The type returned depends on configuration - GlimpsePerformanceConfiguration.Instance.StorageProvider.
        /// </summary>
        /// <returns>An instance implementing the IStorageProvider interface.</returns>
        public static IStorageProvider GetStorageProvider() 
        {
            if (storageProvider == null) 
            {
                var type = Type.GetType(GlimpsePerformanceConfiguration.Instance.StorageProvider);
                storageProvider = (IStorageProvider)Activator.CreateInstance(type.Assembly.FullName, type.FullName).Unwrap();
            }

            return storageProvider;
        }
    }
}
