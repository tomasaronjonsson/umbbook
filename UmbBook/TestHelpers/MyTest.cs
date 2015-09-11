using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit;
using NUnit.Framework;
using Moq;
using Umbraco.Core.Models;

//testing

namespace UmbBook.TestHelpers
{
    [TestFixture]
    public class MyTest
    {

        #region Tests for the TesteHelper

        /// <summary>
        /// Testing if the Mocked Applicatiocontext is working
        /// </summary>
        [TestCase]
        public void TestMockApplicationContext()
        {
            var applicationContext = TestHelper.MockApplicationContext();

            Assert.IsNotNull(applicationContext);
        }

        /// <summary>
        /// Testing if the mocked umbracocontext is working
        /// </summary>
        [TestCase]
        public void TestMockUmbracoContext()
        {
            var applicationContext = TestHelper.MockApplicationContext();


            var umbracoContext = TestHelper.MockUmbracoContext(applicationContext);

            Assert.IsNotNull(umbracoContext);
        }

        /// <summary>
        /// Testing if the mocked IpublishedContent is not null and has a id of 1000
        /// </summary>
        [TestCase]
        public void TestMockContent()
        {
            var content = TestHelper.MockContent();

            Assert.IsNotNull(content);
            Assert.AreEqual(content.Id, 1000);
        }


        #endregion

        #region controllers tests

        [TestCase]
        public void AcceptedFriendsFeedSurfaceController()
        {
            //create application
            var applicationContext = TestHelper.MockApplicationContext();
            //createa umbraco context
            var umbracoContext = TestHelper.MockUmbracoContext(applicationContext);



            //createa controller to test we mock the services he uses
            var controllerToTest = new UmbBook.Controllers.AcceptedFriendsFeedSurfaceController(umbracoContext, Mock.Of<UmbBook.Services.AcceptedFriendsFeedService>());


            TestHelper.SetupControllerContext(umbracoContext, controllerToTest);

            var results = controllerToTest.renderAccptedFeed();


            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(System.Web.Mvc.ActionResult), results);

        }


        #endregion

        #region test content

        [TestCase]
        public void TestContent()
        {
            //get our list of content
            var listOfContent = TestHelper.getCachedContentAsTestContent("E:/Taj/Code/UmbBook/UmbBook/App_Data/umbraco.config");

            //for debugging and information
            System.Console.WriteLine("Number of content objects found: " + listOfContent.Count);
            //lets check each item
            foreach (var item in listOfContent)
            {
                //this is for a custom property, if the content alias is comment it should have a custom property called commentContent
                if (item.DocumentTypeAlias == "comment")
                {
                    System.Console.Write(item["commentContent"]);
                    Assert.IsNotNullOrEmpty(item["commentContent"].ToString());

                }

                Assert.AreNotEqual(item.Id,0);
            }
        }

        #endregion

    }

}