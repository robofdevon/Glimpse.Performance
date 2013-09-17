// -----------------------------------------------------------------------
// <copyright file="GlimpsePerformanceConfiguration.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Config
{
    using System;

    [Serializable]
    public class GlimpsePerformanceConfiguration : IGlimpsePerformanceConfiguration
    {
        public bool Enabled
        {
            get { return GlimpsePerformanceConfigurationSection.Instance.Enabled; }
            set { throw new NotSupportedException(); }
        }

        public string StorageProviderFullyQualifiedName
        {
            get { return GlimpsePerformanceConfigurationSection.Instance.StorageProvider; }
            set { throw new NotSupportedException(); }
        }

        public long MaxOutputResultLength
        {
            get { return GlimpsePerformanceConfigurationSection.Instance.MaxResults; }
            set { throw new NotSupportedException(); }
        }

        public long WarningThresholdElapsedMilliseconds
        {
            get { return GlimpsePerformanceConfigurationSection.Instance.WarningThresholdMs; }
            set { throw new NotSupportedException(); }
        }

        public long IgnoreThresholdElapsedMilliseconds
        {
            get { return GlimpsePerformanceConfigurationSection.Instance.IgnoreThresholdMs; }
            set { throw new NotSupportedException(); }
        }
    }
}
