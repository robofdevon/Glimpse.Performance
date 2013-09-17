//-----------------------------------------------------------------------
// <copyright file="PerformanceSnapshotTab.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Glimpse.Performance.Core
{
    using System.Collections.Generic;
    using AspNet.Extensibility;
    using Glimpse.Core.Extensibility;
    using Glimpse.Core.Tab.Assist;
    using Config;
    using Storage;

    //todo: unit tests
    public class PerformanceSnapshotTab : AspNetTab
    {
        public PerformanceSnapshotTab()
        {
            GlimpsePerformanceConfiguration = new GlimpsePerformanceConfiguration();
        }

        public PerformanceSnapshotTab(IGlimpsePerformanceConfiguration glimpsePerformanceConfiguration)
        {
            GlimpsePerformanceConfiguration = glimpsePerformanceConfiguration;
        }

        protected IGlimpsePerformanceConfiguration GlimpsePerformanceConfiguration;
        public const string TabName = "Performance";
        private IStorageProvider storageProvider;
        private long maxResults;
        private long warningThresholdMs;
        private long ignoreThresholdMs;

        public override string Name
        {
            get { return TabName; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public override object GetData(ITabContext context)
        {
            this.InitialiseStorageProvider();
            this.InitialiseSettings();
            var performanceData = this.GetPerformanceData(this.storageProvider.Retrieve());
            this.storageProvider.Clear();
            return performanceData;
        }

        protected virtual void InitialiseStorageProvider()
        {
            if (storageProvider == null)
            {
                storageProvider = GetStorageFactory().Get();
            }
        }

        //todo: dependency inversion
        protected virtual IStorageFactory GetStorageFactory()
        {
            return new StorageFactory();
        }

        protected virtual void InitialiseSettings()
        {
            this.maxResults = GlimpsePerformanceConfiguration.MaxOutputResultLength;
            this.warningThresholdMs = GlimpsePerformanceConfiguration.WarningThresholdElapsedMilliseconds;
            this.ignoreThresholdMs = GlimpsePerformanceConfiguration.IgnoreThresholdElapsedMilliseconds;
        }

        protected virtual void AddPluginRow(TabSection tabSection, Aspect.MethodInvocation methodInformation)
        {
            tabSection.AddRow()
                    .Column(methodInformation.ClassName)
                    .Column(methodInformation.MethodName)
                    .Column(methodInformation.Parameters)
                    .Column(methodInformation.InvocationTimeMilliseconds)
                    .WarnIf(methodInformation.InvocationTimeMilliseconds >= this.warningThresholdMs);
        }

        protected virtual TabSection GetGlimpseTabSection(
            IEnumerable<Aspect.MethodInvocation> performanceInformation)
        {
            var glimpsePlugin = Plugin.Create("ClassName", "MethodName", "Parameters", "Execution Time (ms)");

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

        protected virtual object GetPerformanceData(IEnumerable<Aspect.MethodInvocation> performanceInformation)
        {
            if (performanceInformation == null)
            {
                return GlimpsePerformanceConfiguration.Enabled
                           ? "No performance data available. Please check your configuration and PostSharp setup to ensure advice is being weaved into your target assemblies. Refer to documentation for more information - http://walkernet.org.uk/glimpse-performance/docs/"
                           : "Glimpse preformance module is not enabled in the config. See the glimpsePerformanceConfiguration.enabled config section attribute.";
            }

            return this.GetGlimpseTabSection(performanceInformation);
        }
    }
}
