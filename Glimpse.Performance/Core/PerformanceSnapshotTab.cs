//-----------------------------------------------------------------------
// <copyright file="PerformanceSnapshotTab.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Core
{
    using Glimpse.AspNet.Extensibility;
    using Glimpse.Core.Extensibility;
    using Glimpse.Core.Tab.Assist;
    using Glimpse.Performance.Aspect;
    using Glimpse.Performance.Config;
    using Glimpse.Performance.Factory;
    using Glimpse.Performance.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An <see cref="AspNetTab"/> glimpse tab plugin for displaying server side method performance information.
    /// </summary>
    public class PerformanceSnapshotTab : AspNetTab
    {
        /// <summary>
        /// The display name for the tab in glimpse.
        /// </summary>
        public const string TabName = "Performance";
        
        // Cache for request lifetime

        /// <summary>
        /// The storage provider used to collect the performance data. This can be overridden in the config.
        /// </summary>
        private IStorageProvider storageProvider;

        /// <summary>
        /// The maximum number of method results to send to the browser. This is be overridden in the config. 
        /// </summary>
        private long maxResults;

        /// <summary>
        /// A threshold for warning messages. If a method performs worse than this figure, it is marked as a warning.
        /// This can be overridden in the config.
        /// </summary>
        private long warningThresholdMs;

        /// <summary>
        /// A threshold for ignoring methods. If a method performs better than this figure, it will not be sent to the browser.
        /// This can be overridden in the config.
        /// </summary>
        private long ignoreThresholdMs;

        /// <summary>
        /// Gets the name of the tab, used by glimpse for the tab header.
        /// </summary>
        public override string Name
        {
            get { return TabName; }
        }

        /// <summary>
        /// Gets the runtime event - in this context we want to process on the end request, after all methods have finished executing.
        /// </summary>
        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        /// <summary>
        /// Used by glimpse core to get information to display down to the browser.
        /// </summary>
        /// <param name="context">The tab context.</param>
        /// <returns>The object for the glimpse core to render to the browser.</returns>
        public override object GetData(ITabContext context)
        {
            // Setup the storage provider, ready for reading the methid data.
            if (this.storageProvider == null)
            {
                this.storageProvider = StorageFactory.GetStorageProvider();
            }

            // Pull in configured settings.
            this.maxResults = GlimpsePerformanceConfiguration.Instance.MaxResults;
            this.warningThresholdMs = GlimpsePerformanceConfiguration.Instance.WarningThresholdMs;
            this.ignoreThresholdMs = GlimpsePerformanceConfiguration.Instance.IgnoreThresholdMs;

            // Get the collection of method results.
            var methodInfoCollection = this.storageProvider.Retrieve();

            // If no results are found, feed back to glimpse.
            if (methodInfoCollection == null) 
            {
                return GlimpsePerformanceConfiguration.Instance.Enabled ?
                    "No performance data available. Please check your configuration and PostSharp setup to ensure advice is being weaved into your target assemblies. Refer to documentation for more information - http://walkernet.org.uk/glimpse-performance/docs/" :
                    "Glimpse preformance module is not enabled in the config. See the glimpsePerformanceConfiguration.enabled config section attribute.";
            }

            // Init the tab section plugin, setting up the columns.
            var plugin = Plugin.Create("Class", "Method", "Parameters", "Execution Time (ms)");

            // Iterate over each method info element
            // checking settings and applying styles element by element.
            int outputCount = 0;
            foreach (var methodInfo in methodInfoCollection)
            {
                if (methodInfo.InvocationTimeMilliseconds > this.ignoreThresholdMs) 
                {
                    if (outputCount < this.maxResults)
                    {
                        plugin.AddRow()
                          .Column(methodInfo.Class)
                          .Column(methodInfo.Method)
                          .Column(methodInfo.Params)
                          .Column(methodInfo.InvocationTimeMilliseconds)
                          .WarnIf(methodInfo.InvocationTimeMilliseconds >= this.warningThresholdMs);

                        outputCount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return plugin;
        }
    }
}
