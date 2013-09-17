//-----------------------------------------------------------------------
// <copyright file="StorageFactory.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Storage
{
    using Config;
    using System;

    //todo: unit tests
    public class StorageFactory : IStorageFactory
    {
        private IStorageProvider _storageProvider;
        private IGlimpsePerformanceConfiguration _glimpsePerformanceConfiguration = new GlimpsePerformanceConfiguration();

        public StorageFactory() : this(new GlimpsePerformanceConfiguration())
        {

        }

        public StorageFactory(IGlimpsePerformanceConfiguration glimpsePerformanceConfiguration)
        {
            _glimpsePerformanceConfiguration = glimpsePerformanceConfiguration;
        }

        public IStorageProvider Get() 
        {
            if (_storageProvider == null) 
            {
                var type = Type.GetType(_glimpsePerformanceConfiguration.StorageProviderFullyQualifiedName);
                _storageProvider = (IStorageProvider)Activator.CreateInstance(type.Assembly.FullName, type.FullName).Unwrap();
            }

            return _storageProvider;
        }
    }
}
