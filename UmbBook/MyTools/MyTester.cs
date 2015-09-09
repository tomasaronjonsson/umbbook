//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using UmbBook.Controllers;
//using Umbraco.Web;

////for testing
//using Umbraco.Tests.TestHelpers;

//namespace UmbBook.MyTools
//{
//    [DatabaseTestBehavior(DatabaseBehavior.NoDatabasePerFixture)]
//    public class MyTester : BaseRoutingTest
//    {

//        private UmbracoContext umbracoContext;

//        public void SetUp()
//        {
//           this.umbracoContext = GetRoutingContext("/").UmbracoContext;
//        }



//        public void PassingTest()
//        {

//            Assert.AreEqual(4, 4);
//        }

//        [Test]
//        public void FailingTest()
//        {
//            Assert.AreEqual(5, 2);
//        }

//        /// <summary>
//        /// Testing MyHelper
//        /// </summary>
//        [Test]
//        public void TestingMyHelper()
//        {
//            //createa a controller to test
//            FeedListSurfaceController controllerToTest = new FeedListSurfaceController();

//            //testing if he returns a value that is not null
//            Assert.NotNull(controllerToTest.RenderFeedListAll());
//        }

//    }
//}