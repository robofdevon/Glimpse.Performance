//-----------------------------------------------------------------------
// <copyright file="GlimpsePerformanceConfiguration.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Config
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Web.Configuration;

    /// <summary>
    /// A custom configuration section which represents various glimpse performance plugin config options.
    /// </summary>
    public sealed class GlimpsePerformanceConfiguration :
        ConfigurationSection
    {
        /// <summary>
        /// The enabled property. 
        /// </summary>
        private static readonly ConfigurationProperty enabled =
            new ConfigurationProperty(
                "enabled",
                typeof(bool), 
                true,
                ConfigurationPropertyOptions.IsRequired);

        /// <summary>
        /// The storageProvider property.
        /// </summary>
        private static readonly ConfigurationProperty storageProvider =
            new ConfigurationProperty(
                "storageProvider",
                typeof(string), 
                "Glimpse.Performance.Provider.HttpContextStorageProvider, Glimpse.Performance",
                ConfigurationPropertyOptions.None);

        /// <summary>
        /// The MaxResults property. 
        /// </summary>
        private static readonly ConfigurationProperty maxResults =
            new ConfigurationProperty(
                "maxResults",
                typeof(long), 
                (long)100,
                ConfigurationPropertyOptions.None);

        /// <summary>
        /// The WarningThresholdMs property. 
        /// </summary>
        private static readonly ConfigurationProperty warningThresholdMs =
            new ConfigurationProperty(
                "warningThresholdMs",
                typeof(long), 
                (long)100,
                ConfigurationPropertyOptions.None);

        /// <summary>
        /// The 'IgnoreThresholdMs' property. 
        /// </summary>
        private static readonly ConfigurationProperty ignoreThresholdMs =
            new ConfigurationProperty(
                "ignoreThresholdMs",
                typeof(long),
                (long)0,
                ConfigurationPropertyOptions.None);

        /// <summary>
        /// The singleton instance of the custom configuration section.
        /// </summary>
        private static volatile GlimpsePerformanceConfiguration instance;

        /// <summary>
        /// An object used for concurrency control.
        /// </summary>
        private static object syncRoot = new object();

        /// <summary>
        /// The collection (property bag) that contains 
        /// the section properties. 
        /// </summary>
        private static ConfigurationPropertyCollection properties;

        /// <summary>
        /// Internal flag to disable 
        /// property setting.
        /// </summary>
        private static bool readOnly;

        /// <summary>
        /// Prevents a default instance of the <see cref="GlimpsePerformanceConfiguration" /> class from being created.
        /// Initializes a new instance of <see cref="GlimpsePerformanceConfiguration" />, adding the configurable attributes
        /// to the properties collection.
        /// </summary>
        private GlimpsePerformanceConfiguration()
        {
            // Property initialization
            properties =
                new ConfigurationPropertyCollection();

            properties.Add(enabled);
            properties.Add(storageProvider);
            properties.Add(maxResults);
            properties.Add(warningThresholdMs);
            properties.Add(ignoreThresholdMs);
        }

        /// <summary>
        /// Gets the singleton instance for the GlimpsePerformanceConfiguration type.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the glimpse performance plugin is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }

            set
            {
                // With this you disable the setting. 
                // Remember that the readOnly flag must 
                // be set to true in the GetRuntimeObject.
                this.ThrowIfReadOnly("enabled");
                this["enabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the storage provider to be used.
        /// By default this is set to "Glimpse.Performance.Provider.HttpContextStorageProvider, Glimpse.Performance".
        /// </summary>
        public string StorageProvider
        {
            get
            {
                return (string)this["storageProvider"];
            }

            set
            {
                // With this you disable the setting. 
                // Remember that the readOnly flag must 
                // be set to true in the GetRuntimeObject.
                this.ThrowIfReadOnly("enabled");
                this["storageProvider"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the MaxResults property.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the WarningThresholdMs property.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the IgnoreThresholdMs property.
        /// </summary>
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

        /// <summary>
        /// Gets the initialized property bag.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the custom configuration is read only.
        /// </summary>
        private new bool IsReadOnly
        {
            get
            {
                return readOnly;
            }
        }

        /// <summary>
        /// Customizes the use of CustomSection
        /// by setting readOnly to false.
        /// Remember you must use it along with ThrowIfReadOnly. 
        /// </summary>
        /// <returns>A custom object.</returns>
        protected override object GetRuntimeObject()
        {
            // To enable property setting just assign true to 
            // the following flag.
            readOnly = false;
            return base.GetRuntimeObject();
        }

        /// <summary>
        /// Used to disable a property setting.
        /// </summary>
        /// <param name="propertyName">The name of the property to test.</param>
        private void ThrowIfReadOnly(string propertyName)
        {
            if (this.IsReadOnly)
            {
                throw new ConfigurationErrorsException(
                    "The property " + propertyName + " is read only.");
            }
        }
    }
}