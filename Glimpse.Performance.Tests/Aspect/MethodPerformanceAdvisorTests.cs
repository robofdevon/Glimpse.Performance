// -----------------------------------------------------------------------
// <copyright file="MethodPerformanceAdvisorTests.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Glimpse.Performance.Conversion;

namespace Glimpse.Performance.Aspect
{
    using System.Reflection;
    using Config;
    using Storage;
    using Time;
    using Moq;
    using NUnit.Framework;
    using PostSharp.Aspects;

    [TestFixture]
    public class MethodPerformanceAdvisorTests
    {
        protected Mock<IGlimpsePerformanceConfiguration> GlimpsePerformanceConfigurationMock;
        protected Mock<IStopwatch> StopwatchMock;
        protected Mock<IStorageProvider> StorageProviderMock;
        protected Mock<IStorageFactory> StorageFactoryMock;
        protected Mock<IConversionHelper> ConversionHelperMock;

        [SetUp]
        public void SetUp()
        {
            GlimpsePerformanceConfigurationMock = new Mock<IGlimpsePerformanceConfiguration>();
            StopwatchMock = new Mock<IStopwatch>();
            ConversionHelperMock = new Mock<IConversionHelper>();
            StorageProviderMock = new Mock<IStorageProvider>();
            StorageFactoryMock = new Mock<IStorageFactory>();
            StorageFactoryMock.Setup(m => m.Get())
                              .Returns(StorageProviderMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            GlimpsePerformanceConfigurationMock = null;
            StopwatchMock = null;
            StorageProviderMock = null;
            ConversionHelperMock = null;
            StorageFactoryMock = null;
        }

        [Test]
        public void OnEntry_StartsStopwatch()
        {
            //Arrange
            var methodPerformanceAdvisor = GetPerformanceAdvisorWithInjectedMocks();

            var methodExecutionArgs = new MethodExecutionArgs(null, null);

            //Act
            methodPerformanceAdvisor.OnEntry(methodExecutionArgs);

            //Assert
            StopwatchMock.Verify(m => m.Start(), Times.Exactly(1));
        }

        [Test]
        public void OnEntry_Disabled_DoesNotSetElapsedTime()
        {
            //Arrange
            var expectedDurationMilliseconds = 10L;
            StopwatchMock.Setup(m => m.ElapsedMilliseconds)
                         .Returns(expectedDurationMilliseconds);

            var methodPerformanceAdvisor = GetPerformanceAdvisorWithInjectedMocks();
            var methodExecutionArgs = new MethodExecutionArgs(null, null);

            //Act
            methodPerformanceAdvisor.OnEntry(methodExecutionArgs);

            //Assert
            Assert.IsNull(methodExecutionArgs.MethodExecutionTag);
        }

        [Test]
        public void OnEntryMethod_Enabled_SetsElapsedTime()
        {
            //Arrange
            var expectedDurationMilliseconds = 10L;
            StopwatchMock.Setup(m => m.ElapsedMilliseconds)
                         .Returns(expectedDurationMilliseconds);

            GlimpsePerformanceConfigurationMock.Setup(m => m.Enabled)
                                               .Returns(true);

            var methodPerformanceAdvisor = GetPerformanceAdvisorWithInjectedMocks();
            var methodExecutionArgs = new MethodExecutionArgs(null, null);
            methodExecutionArgs.Method = GetMethod_MethodBase();

            //Act
            methodPerformanceAdvisor.OnEntry(methodExecutionArgs);

            //Assert
            Assert.That(((long)methodExecutionArgs.MethodExecutionTag) == expectedDurationMilliseconds);
        }

        [Test]
        public void OnEntryProperty_Enabled_DoesNotSetElapsedTime()
        {
            //Arrange
            var expectedDurationMilliseconds = 10L;
            StopwatchMock.Setup(m => m.ElapsedMilliseconds)
                         .Returns(expectedDurationMilliseconds);

            GlimpsePerformanceConfigurationMock.Setup(m => m.Enabled)
                                               .Returns(true);

            var methodPerformanceAdvisor = GetPerformanceAdvisorWithInjectedMocks();
            var methodExecutionArgs = new MethodExecutionArgs(null, null);
            methodExecutionArgs.Method = GetProperty_MethodBase;

            //Act
            methodPerformanceAdvisor.OnEntry(methodExecutionArgs);

            //Assert
            Assert.IsNull(methodExecutionArgs.MethodExecutionTag);
        }

        [Test]
        public void OnExitMethod_ReadsStopwatch()
        {
            //Arrange
            var startDuration = 10L;
            var endDuration = 100L;
            var expectedElapsedDuration = 90L;

            MethodInvocation storedMethodInvocation = null;

            StopwatchMock.Setup(m => m.ElapsedMilliseconds)
                         .Returns(endDuration);

            GlimpsePerformanceConfigurationMock.Setup(m => m.Enabled)
                         .Returns(true);

            ConversionHelperMock.Setup(m => m.ToMethodInvocation(It.IsAny<MethodExecutionArgs>(), It.IsAny<long>()))
                         .Returns(new MethodInvocation
                             {
                                 ClassName = "Glimpse.Performance.Aspect.MethodPerformanceAdvisorTests",
                                 InvocationTimeMilliseconds = expectedElapsedDuration,
                                 MethodName = "GetMethod_MethodBase",
                                 Parameters = ""
                             });

            StorageProviderMock.Setup(m => m.Store(It.IsAny<MethodInvocation>()))
                         .Callback<MethodInvocation>(mi => storedMethodInvocation = mi);

            var methodPerformanceAdvisor = GetPerformanceAdvisorWithInjectedMocks();
            var methodExecutionArgs = new MethodExecutionArgs(null, null);
            methodExecutionArgs.Method = GetMethod_MethodBase();

            //Act
            //must do onentry to set enabled
            methodPerformanceAdvisor.OnEntry(methodExecutionArgs);
            //reset execution tag to start time
            methodExecutionArgs.MethodExecutionTag = startDuration;
            methodPerformanceAdvisor.OnExit(methodExecutionArgs);

            //Assert
            Assert.That(storedMethodInvocation.InvocationTimeMilliseconds == expectedElapsedDuration);
        }

        protected MethodBase GetMethod_MethodBase()
        {
            return MethodBase.GetCurrentMethod();
        }

        protected MethodBase GetProperty_MethodBase
        {
            get
            {
                return MethodBase.GetCurrentMethod();
            }
        }

        protected MethodPerformanceAdvisor GetPerformanceAdvisorWithInjectedMocks()
        {
            return new MethodPerformanceAdvisor(StopwatchMock.Object,
                                                GlimpsePerformanceConfigurationMock.Object,
                                                StorageFactoryMock.Object,
                                                ConversionHelperMock.Object);
        }
    }
}
