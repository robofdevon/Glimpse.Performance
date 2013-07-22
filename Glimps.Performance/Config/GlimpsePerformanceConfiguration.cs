using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace Glimpse.Performance.Config
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GlimpsePerformanceConfiguration :
        ConfigurationSection
    {
        private static volatile GlimpsePerformanceConfiguration instance;
        private static object syncRoot = new Object();

        public static GlimpsePerformanceConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance =
                                ConfigurationManager
                                    .GetSection("glimpsePerformanceConfiguration")
                                    as GlimpsePerformanceConfiguration;

                            if (instance == null) 
                            {
                                instance = new GlimpsePerformanceConfiguration();
                            }
                        }   
                    }
                }

                return instance;
            }
        }

        // The collection (property bag) that contains  
        // the section properties. 
        private static ConfigurationPropertyCollection _Properties;

        // Internal flag to disable  
        // property setting. 
        private static bool _ReadOnly;

        // The enabled property. 
        private static readonly ConfigurationProperty _enabled =
            new ConfigurationProperty("enabled",
            typeof(bool), true,
            ConfigurationPropertyOptions.IsRequired);

        // The storageProvider property. 
        private static readonly ConfigurationProperty _storageProvider =
            new ConfigurationProperty("storageProvider",
            typeof(string), "Glimpse.Performance.Provider.HttpContextStorageProvider, Glimpse.Performance",
            ConfigurationPropertyOptions.None);

        // The MaxResults property. 
        private static readonly ConfigurationProperty _maxResults =
            new ConfigurationProperty("maxResults",
            typeof(long), (long)100,
            ConfigurationPropertyOptions.None);

        // The WarningThresholdMs property. 
        private static readonly ConfigurationProperty _warningThresholdMs =
            new ConfigurationProperty("warningThresholdMs",
            typeof(long), (long)100,
            ConfigurationPropertyOptions.None);

        // CustomSection constructor. 
        private GlimpsePerformanceConfiguration()
        {
            // Property initialization
            _Properties =
                new ConfigurationPropertyCollection();

            _Properties.Add(_enabled);
            _Properties.Add(_storageProvider);
            _Properties.Add(_maxResults);
            _Properties.Add(_warningThresholdMs);
        }


        // This is a key customization.  
        // It returns the initialized property bag. 
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _Properties;
            }
        }


        private new bool IsReadOnly
        {
            get
            {
                return _ReadOnly;
            }
        }

        // Use this to disable property setting. 
        private void ThrowIfReadOnly(string propertyName)
        {
            if (IsReadOnly)
                throw new ConfigurationErrorsException(
                    "The property " + propertyName + " is read only.");
        }


        // Customizes the use of CustomSection 
        // by setting _ReadOnly to false. 
        // Remember you must use it along with ThrowIfReadOnly. 
        protected override object GetRuntimeObject()
        {
            // To enable property setting just assign true to 
            // the following flag.
            _ReadOnly = false;
            return base.GetRuntimeObject();
        }

        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                // With this you disable the setting. 
                // Remember that the _ReadOnly flag must 
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("enabled");
                this["enabled"] = value;
            }
        }

        public string StorageProvider
        {
            get
            {
                return (string)this["storageProvider"];
            }
            set
            {
                // With this you disable the setting. 
                // Remember that the _ReadOnly flag must 
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("enabled");
                this["storageProvider"] = value;
            }
        }

        [LongValidator(MinValue = 1, MaxValue = 1000000,
            ExcludeRange = false)]
        public long MaxResults
        {
            get
            {
                return (long)this["maxResults"];
            }
            set
            {
                this["maxResults"] = value;
            }
        }

        [LongValidator(MinValue = 1, MaxValue = 1000000,
            ExcludeRange = false)]
        public long WarningThresholdMs
        {
            get
            {
                return (long)this["warningThresholdMs"];
            }
            set
            {
                this["warningThresholdMs"] = value;
            }
        }
    }
}