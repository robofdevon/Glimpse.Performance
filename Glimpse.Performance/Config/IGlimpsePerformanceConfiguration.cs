// -----------------------------------------------------------------------
// <copyright file="IGlimpsePerformanceConfiguration.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Config
{
    public interface IGlimpsePerformanceConfiguration
    {
        bool Enabled { get; set; }
        string StorageProviderFullyQualifiedName { get; set; }
        long MaxOutputResultLength { get; set; }
        long WarningThresholdElapsedMilliseconds { get; set; }
        long IgnoreThresholdElapsedMilliseconds { get; set; }
    }
}
