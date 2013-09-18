// -----------------------------------------------------------------------
// <copyright file="HttpContextStorageProviderTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Web;
using System.Web.Configuration;
using Glimpse.Performance.Aspect;
using NUnit.Framework;

namespace Glimpse.Performance.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class HttpContextStorageProviderTests
    {
        //store, retreive, clear
        private HttpContext httpContext;
        private IStorageProvider storageProvider;
        private const string storageKey = "_methodInfo";

        [SetUp]
        public void Setup()
        {
            var request = new HttpRequest("", "http://tempuri.org", "");
            var response = new HttpResponse(new StringWriter());
            httpContext = new HttpContext(request, response);
            storageProvider = new HttpContextStorageProvider(httpContext);
        }

        [TearDown]
        public void TearDown()
        {
            httpContext = null;
            storageProvider = null;
        }

        [Test]
        public void HttpContextStorageProvider_Store_AddsElementToStorage()
        {
            //Arrange
            var expectedMethodName = "HelloWorld";
            var methodInvocationToStore = new MethodInvocation { MethodName = expectedMethodName };

            //Act
            storageProvider.Store(methodInvocationToStore);

            //Assert
            Assert.That(((Stack<MethodInvocation>)httpContext.Items[storageKey])
                  .Pop()
                  .MethodName == expectedMethodName);
        }

        [Test]
        public void HttpContextStorageProvider_Store_RetrieveElementFromStorage()
        {
            //Arrange
            var expectedMethodName = "HelloWorld";
            var methodInvocationToStore = new MethodInvocation { MethodName = expectedMethodName };
            storageProvider.Store(methodInvocationToStore);

            //Act
            MethodInvocation retrievedMethodInvocationElement = null;
            foreach (var element in storageProvider.Retrieve())
            {
                retrievedMethodInvocationElement = element;
            }

            //Assert
            Assert.That(retrievedMethodInvocationElement.MethodName == expectedMethodName);
        }

        [Test]
        public void HttpContextStorageProvider_Clear_ClearsStorageCollection()
        {
            //Arrange
            var methodInvocationToStore = new MethodInvocation();
            storageProvider.Store(methodInvocationToStore);

            //Act
            storageProvider.Clear();

            //Assert
            Assert.That(((Stack<MethodInvocation>)httpContext.Items[storageKey]).Count == 0);
        }
    }
}
