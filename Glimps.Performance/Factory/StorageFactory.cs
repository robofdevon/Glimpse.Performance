using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Performance.Interface;
using Glimpse.Performance.Config;

namespace Glimpse.Performance.Factory
{
    public static class StorageFactory
    {
        //Cache per web request
        private static IStorageProvider storageProvider;

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
