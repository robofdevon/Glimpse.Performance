// -----------------------------------------------------------------------
// <copyright file="ConversionHelperTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Conversion
{
    using NUnit.Framework;
    using System.Reflection;
    using PostSharp.Aspects;

    [TestFixture]
    public class ConversionHelperTests
    {
        [Test]
        public void ConversionHelper_ConvertsMethodExecutionArgs_MethodInvocation()
        {
            //Arrange
            var expectedInvocationTime = 100L;
            var expectedMethodName = "MyMethod";
            var expectedClassName = "Glimpse.Performance.Conversion.ConversionHelperTests";
            var expectedParams = "Int32 Param1, Int32 Param2";

            var methodExecutionArgs = new MethodExecutionArgs(null, null);
            methodExecutionArgs.Method = MyMethod(1, 2);
            IConversionHelper conversionHelper = new ConversionHelper();

            //Act
            var methodInvocation = conversionHelper.ToMethodInvocation(methodExecutionArgs, expectedInvocationTime);

            //Assert
            Assert.That(methodInvocation.InvocationTimeMilliseconds == expectedInvocationTime);
            Assert.That(methodInvocation.MethodName == expectedMethodName);
            Assert.That(methodInvocation.ClassName == expectedClassName);
            Assert.That(methodInvocation.Parameters == expectedParams);
        }

        private MethodBase MyMethod(int Param1, int Param2)
        {
            return MethodBase.GetCurrentMethod();
        }
    }

    
}
