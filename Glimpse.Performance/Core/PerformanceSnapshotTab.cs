//-----------------------------------------------------------------------
// <copyright file="PerformanceSnapshotTab.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Core
{
    using System.Collections.Generic;
    using Glimpse.AspNet.Extensibility;
    using Glimpse.Core.Extensibility;
    using Glimpse.Core.Tab.Assist;
    using Glimpse.Performance.Config;
    using Glimpse.Performance.Factory;
    using Glimpse.Performance.Interface;

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
            this.InitialiseStorageProvider();
            this.InitialiseSettings();
            var performanceData = this.GetPerformanceData(this.storageProvider.Retrieve());
            this.storageProvider.Clear();
            return performanceData;
        }

        /// <summary>
        /// Initializes the local storage provider property, if it has not been initialized already.
        /// </summary>
        protected virtual void InitialiseStorageProvider()
        {
            // Setup the storage provider, ready for reading the methid data.
            if (this.storageProvider == null)
            {
                this.storageProvider = StorageFactory.GetStorageProvider();
            }
        }

        /// <summary>
        /// Initializes the instance with configuration settings.
        /// </summary>
        protected virtual void InitialiseSettings()
        {
            this.maxResults = GlimpsePerformanceConfiguration.Instance.MaxResults;
            this.warningThresholdMs = GlimpsePerformanceConfiguration.Instance.WarningThresholdMs;
            this.ignoreThresholdMs = GlimpsePerformanceConfiguration.Instance.IgnoreThresholdMs;
        }

        /// <summary>
        /// Adds a row to the tab section, representing the <see cref="MethodInvocationInformation"/> instance passed.
        /// </summary>
        /// <param name="tabSection">The tabs section to add the row to.</param>
        /// <param name="methodInformation">The data that will makeup the new row.</param>
        protected virtual void AddPluginRow(TabSection tabSection, Aspect.MethodInvocationInfo methodInformation)
        {
            tabSection.AddRow()
                    .Column(methodInformation.Class)
                    .Column(methodInformation.Method)
                    .Column(methodInformation.Params)
                    .Column(methodInformation.InvocationTimeMilliseconds)
                    .WarnIf(methodInformation.InvocationTimeMilliseconds >= this.warningThresholdMs);
        }

        /// <summary>
        /// Gets a new glimpse tab section from the specified collection of <see cref="Aspect.MethodInvocationInfo"/>.
        /// </summary>
        /// <param name="performanceInformation">The performance information.</param>
        /// <returns>A Glimpse tag section</returns>
        protected virtual TabSection GetGlimpseTabSection(
            IEnumerable<Aspect.MethodInvocationInfo> performanceInformation)
        {
            var glimpsePlugin = Plugin.Create("Class", "Method", "Parameters", "Execution Time (ms)");

            int rowsAdded = 0;
            foreach (var methodInformation in performanceInformation)
            {
                if (methodInformation.InvocationTimeMilliseconds > this.ignoreThresholdMs)
                {
                    if (rowsAdded < this.maxResults)
                    {
                        this.AddPluginRow(glimpsePlugin, methodInformation);
                        rowsAdded++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return glimpsePlugin;
        }

        /// <summary>
        /// Gets the performance data object from the specified collection of <see cref="Aspect.MethodInvocationInfo"/>.
        /// </summary>
        /// <param name="performanceInformation">A collection of performance data.</param>
        /// <returns>An object which glimpse can render in the web browser.</returns>
        protected virtual object GetPerformanceData(IEnumerable<Aspect.MethodInvocationInfo> performanceInformation)
        {
            if (performanceInformation == null)
            {
                return GlimpsePerformanceConfiguration.Instance.Enabled
                           ? "No performance data available. Please check your configuration and PostSharp setup to ensure advice is being weaved into your target assemblies. Refer to documentation for more information - http://walkernet.org.uk/glimpse-performance/docs/"
                           : "Glimpse preformance module is not enabled in the config. See the glimpsePerformanceConfiguration.enabled config section attribute.";
            }

            return this.GetGlimpseTabSection(performanceInformation);
        }
    }
}
