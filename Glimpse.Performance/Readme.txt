# Glimpse 1.0.0 Performance Plugin

### What is Glimpse and the Performance Plugin?

Glimpse is an awesome open source Firebug for ASP.NET: http://getglimpse.com/

### What is this?

Glimpse performance plugin is a Glimpse extension that allows you to monitor the performance of your application via Glimpse.

### How does it work?

The Glimpse performance plugin uses an AOP advisor to gather performance information about your application, and then makes it available to Glimpse.
The level of information captured by the Glimpse Performance Plugin is configurable at compile and runtime.

### Quick start

Please find setup info here - http://walkernet.org.uk/glimpse-performance/docs/

4. The Glimpse Performance plugin will have added a new configuration section to your web.config. Please set the attributes on
the glimpsePerformanceConfiguration section as you with. The attributes are;
- enabled = (true/false) turn monitoring on/off,
- maxResults = controls the number of results sent down to the browser. By default results are stored in a stack structure, last in - first out.
- warningThresholdMs = any methods taking >= to this limit to execute (including children), will be marked with a warning.
- ignoreThresholdMs = any method taking <= to this limit to execute (including children), will not be sent down to the browser.
- storageProvider = the inbuilt storage provider can be overriden by specifying a fully qualified type that implements the 
Glimpse.Performance.Interface.IStorageProvider interface. By default the storage provider used is "Glimpse.Performance.Provider.HttpContextStorageProvider, Glimpse.Performance".


### How much overhead will this add to my project, and can I run this in production?
I am still at a relatively early stage with this project. I will try to answer these questions shortly.