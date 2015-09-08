using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UmbBook.Controllers;
using Umbraco.Web;

//for testing
using Umbraco.Tests.TestHelpers;
using NUnit.Framework;

namespace UmbBook.MyTools
{
    [TestFixture]
    [DatabaseTestBehavior(DatabaseBehavior.NoDatabasePerFixture)]
    public class MyTester : BaseUmbracoApplicationTest
    {

        private UmbracoContext umbracoContext;

        [SetUp]
        public void SetUp()
        {
           // this.umbracoContext = GetRoutingContext("/").UmbracoContext;
        }



        [Test]
        public void PassingTest()
        {

            Assert.AreEqual(4, 4);
        }

        [Test]
        public void FailingTest()
        {
            Assert.AreEqual(5, 2);
        }

        /// <summary>
        /// Testing MyHelper
        /// </summary>
        [Test]
        public void TestingMyHelper()
        {
            //createa a controller to test
            FeedListSurfaceController controllerToTest = new FeedListSurfaceController();

            //testing if he returns a value that is not null
            Assert.NotNull(controllerToTest.RenderFeedListAll());
        }

    }
}