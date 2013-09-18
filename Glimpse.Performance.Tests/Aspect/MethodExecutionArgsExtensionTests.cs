// -----------------------------------------------------------------------
// <copyright file="MethodExecutionArgsExtensionTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using NUnit.Framework;
using PostSharp.Aspects;

namespace Glimpse.Performance.Aspect
{
    [TestFixture]
    public class MethodExecutionArgsExtensionTests
    {
        private MethodExecutionArgs methodExecutionArgs;

        [SetUp]
        public void SetUp()
        {
            var myClass = new MyClass();
            methodExecutionArgs = new MethodExecutionArgs(null, null);
            methodExecutionArgs.Method = myClass.MyMethod(1, "");
        }

        [TearDown]
        public void TearDown()
        {
            methodExecutionArgs = null;
        }

        [Test]
        public void MethodExecutionArgs_GetMethodName_MethodName()
        {
            //Arrange
            var expectedMethodName = "MyMethod";

            //Act
            var methodName = methodExecutionArgs.GetMethodName();

            //Assert
            Assert.That(methodName == expectedMethodName);
        }

        [Test]
        public void MethodExecutionArgs_GetClassName_ClassName()
        {
            //Arrange
            var expectedClassName = "Glimpse.Performance.Aspect.MyClass";

            //Act
            var className = methodExecutionArgs.GetClassName();

            //Assert
            Assert.That(className == expectedClassName);
        }

        [Test]
        public void MethodExecutionArgs_GetParameters_Parameters()
        {
            //Arrange
            var expectedParams = "Int32 i, System.String s";

            //Act
            var parameters = methodExecutionArgs.GetParameterTypesAndNamesCsv();

            //Assert
            Assert.That(parameters == expectedParams);
        }
    }

    public class MyClass
    {
        public MethodBase MyMethod(int i, string s)
        {
            return MethodBase.GetCurrentMethod();
        }
    }
}
