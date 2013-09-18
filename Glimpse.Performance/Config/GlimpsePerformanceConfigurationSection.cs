//-----------------------------------------------------------------------
// <copyright file="GlimpsePerformanceConfiguration.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Config
{
    using System.Configuration;

    public sealed class GlimpsePerformanceConfigurationSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty EnabledConfigurationProperty =
            new ConfigurationProperty(
                "enabled",
                typeof(bool), 
                true,
                ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty StorageProviderConfigurationProperty =
            new ConfigurationProperty(
                "storageProvider",
                typeof(string), 
                "Glimpse.Performance.Storage.HttpContextStorageProvider, Glimpse.Performance",
                ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty MaxResultsConfigurationProperty =
            new ConfigurationProperty(
                "maxResults",
                typeof(long), 
                (long)100,
                ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty WarningThresholdMsConfigurationProperty =
            new ConfigurationProperty(
                "warningThresholdMs",
                typeof(long), 
                (long)100,
                ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty IgnoreThresholdMsConfigurationProperty =
            new ConfigurationProperty(
                "ignoreThresholdMs",
                typeof(long),
                (long)0,
                ConfigurationPropertyOptions.None);

        private static volatile GlimpsePerformanceConfigurationSection _instance;
        private static readonly object SyncRoot = new object();
        private static ConfigurationPropertyCollection _properties;
        private static bool _readOnly;

        private GlimpsePerformanceConfigurationSection()
        {
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(EnabledConfigurationProperty);
            _properties.Add(StorageProviderConfigurationProperty);
            _properties.Add(MaxResultsConfigurationProperty);
            _properties.Add(WarningThresholdMsConfigurationProperty);
            _properties.Add(IgnoreThresholdMsConfigurationProperty);
        }

        public static GlimpsePerformanceConfigurationSection Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitialiseInstance();
                }

                return _instance;
            }
        }

        private static void InitialiseInstance()
        {
            lock (SyncRoot)
            {
                if (_instance == null)
                {
                    _instance =
                        ConfigurationManager
                            .GetSection("glimpsePerformanceConfiguration")
                            as GlimpsePerformanceConfigurationSection;

                    if (_instance == null)
                    {
                        _instance = new GlimpsePerformanceConfigurationSection();
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }

            set
            {
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

        [LongValidator(MinValue = 0, MaxValue = 1000000,
            ExcludeRange = false)]
        public long IgnoreThresholdMs
        {
            get
            {
                return (long)this["ignoreThresholdMs"];
            }

            set
            {
                this["ignoreThresholdMs"] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        private new bool IsReadOnly
        {
            get
            {
                return _readOnly;
            }
        }

        protected override object GetRuntimeObject()
        {
            _readOnly = false;
            return base.GetRuntimeObject();
        }

        private void ThrowIfReadOnly(string propertyName)
        {
            if (IsReadOnly)
            {
                throw new ConfigurationErrorsException(string.Format("The property {0} is read only.", propertyName));
            }
        }
    }
}