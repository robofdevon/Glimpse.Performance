using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using Glimpse.Performance.Aspect;
using Glimpse.Performance.Interface;
using Glimpse.Performance.Factory;
using Glimpse.Performance.Config;

namespace Glimpse.Performance.Core
{
    public class PerformanceSnapshotTab : AspNetTab
    {
        public const string TabName = "Performance";
        
        //Cache for request lifetime
        private IStorageProvider storageProvider;
        private long maxResults;
        private long warningThresholdMs;

        public override object GetData(ITabContext context)
        {
            if (storageProvider == null)
            {
                storageProvider = StorageFactory.GetStorageProvider();
            }

            maxResults = GlimpsePerformanceConfiguration.Instance.MaxResults;
            warningThresholdMs = GlimpsePerformanceConfiguration.Instance.WarningThresholdMs;

            var methodInfoCollection = storageProvider.Retreive();
            if (methodInfoCollection == null) 
            {
                return GlimpsePerformanceConfiguration.Instance.Enabled ?
                    "No performance data available." :
                    "Glimpse preformance module is not enabled in the config. See the glimpsePerformanceConfiguration.enabled config section attribute.";
            }

            var plugin = Plugin.Create("Class", "Method", "Parameters", "Execution Time (ms)");

            int outputCount = 0;
            foreach (var methodInfo in methodInfoCollection)
            {
                if (outputCount < maxResults)
                {
                    plugin.AddRow()
                      .Column(methodInfo.Class)
                      .Column(methodInfo.Method)
                      .Column(methodInfo.Params)
                      .Column(methodInfo.InvocationTimeMilliseconds)
                      .WarnIf(methodInfo.InvocationTimeMilliseconds >= warningThresholdMs); //Warn if method takes longer than x ms to execute

                    outputCount++;
                }
                else 
                {
                    break;
                }
            }

            return plugin;
        }

        public override string Name
        {
            get { return TabName; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }
    }
}
