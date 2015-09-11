using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Profiling;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using System.Collections.Generic;

namespace UmbBook.TestHelpers
{
    public class TestHelper
    {
        /// <summary>
        /// Method to mock an ApplicationContext object for testing
        /// </summary>
        /// <returns>Mocked up ApplicationContext</returns>
        public static ApplicationContext MockApplicationContext()
        {
            return new ApplicationContext(CacheHelper.CreateDisabledCacheHelper(),
                new ProfilingLogger(new Mock<ILogger>().Object, new Mock<IProfiler>().Object));
        }

        /// <summary>
        /// Method to mock an UmbracoContext obect, requires an ApplicationContext which can be
        /// retrieved using the .MockApplicationContext() method
        /// </summary>
        /// <param name="appCtx"></param>
        /// <returns>Mocked up UmbracoContext</returns>
        public static UmbracoContext MockUmbracoContext(ApplicationContext appCtx)
        {
            return UmbracoContext.EnsureContext(
                new Mock<HttpContextBase>().Object,
                appCtx,
                new Mock<WebSecurity>(null, null).Object,
                Mock.Of<IUmbracoSettingsSection>(section => section.WebRouting == Mock.Of<IWebRoutingSection>(routingSection => routingSection.UrlProviderMode == "AutoLegacy")),
                Enumerable.Empty<IUrlProvider>(),
                true);
        }

        /// <summary>
        /// Method to Mock a Ipuslihedcontent for testing purposes with the ID of 1000
        /// </summary>
        /// <returns>IPublishedContent with the Id of 1000</returns>
        public static IPublishedContent MockContent()
        {
            return Mock.Of<IPublishedContent>(publishedContent => publishedContent.Id == 1000);
        }

        /// <summary>
        /// Method to setup the context for a controller requirest both a UmbracoContext and a Controller object
        /// </summary>
        /// <param name="umbCtx">UmbracoContext which can be created using the .MockUmbracoContext() method</param>
        /// <param name="controller">The controller you want to test, this method will set it up for testing</param>
        public static void SetupControllerContext(UmbracoContext umbCtx, Controller controller)
        {
            var webRoutingSettings = Mock.Of<IWebRoutingSection>(section => section.UrlProviderMode == "AutoLegacy");
            var contextBase = umbCtx.HttpContext;
            var pcr = new PublishedContentRequest(new Uri("http://localhost/test"),
                umbCtx.RoutingContext,
                webRoutingSettings,
                s => Enumerable.Empty<string>())
            {
                PublishedContent = MockContent(),
            };

            var routeData = new RouteData();
            var routeDefinition = new RouteDefinition
            {
                PublishedContentRequest = pcr
            };
            routeData.DataTokens.Add("umbraco-route-def", routeDefinition);
            controller.ControllerContext = new ControllerContext(contextBase, routeData, controller);
        }

        /// <summary>
        /// Fetched the cached content from the umbraco config and returns a list of testcontent for testing
        /// </summary>
        /// <param name="filePathToUmbracoConfig">File path to the location of the umbraco.config to extract content from</param>
        /// <returns>Returns a list of TestContent, a class that implements the IPublishedContent from Umbraco</returns>
        public static List<TestContentModel> getCachedContentAsTestContent(string filePathToUmbracoConfig)
        {
            //read the cache
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(filePathToUmbracoConfig);


            //find all nodes with a nodeTypesAlias attribute , which should be a site etc.
            var nodes = xmldoc.SelectNodes("//*[@nodeTypeAlias]");

            //list to store all the content we are going to return
            List<TestContentModel> listOfContent = new List<TestContentModel>();

            //now let's go over all the nodes and extract some information
            foreach (System.Xml.XmlNode node in nodes)
            {
                //creata a dictonary to store all the attributes of the node
                Dictionary<string, string> attributesNameToValue = new Dictionary<string, string>();

                //now we go over all the informormation in the content object and add it to our collection
                foreach (System.Xml.XmlAttribute nodeAttribute in node.Attributes)
                {
                    attributesNameToValue.Add(nodeAttribute.Name, nodeAttribute.Value);

                }
                //ceate a new TestContent to store our information we have collected
                TestContentModel publishedContentToStore = new TestContentModel();
                //was easier to do it this way then to use some mapper
                publishedContentToStore.Id = int.Parse(attributesNameToValue["id"]);
                publishedContentToStore.Level = int.Parse(attributesNameToValue["level"]);
                publishedContentToStore.SortOrder = int.Parse(attributesNameToValue["sortOrder"]);
                publishedContentToStore.CreateDate = DateTime.Parse(attributesNameToValue["createDate"]);
                publishedContentToStore.UpdateDate = DateTime.Parse(attributesNameToValue["updateDate"]);
                publishedContentToStore.Name = attributesNameToValue["nodeName"];
                publishedContentToStore.UrlName = attributesNameToValue["urlName"];
                publishedContentToStore.Path = attributesNameToValue["path"];
                publishedContentToStore.DocumentTypeId = int.Parse(attributesNameToValue["nodeType"]);
                publishedContentToStore.CreatorName = attributesNameToValue["creatorName"];
                publishedContentToStore.WriterName = attributesNameToValue["writerName"];
                publishedContentToStore.WriterId = int.Parse(attributesNameToValue["writerID"]);
                publishedContentToStore.TemplateId = int.Parse(attributesNameToValue["template"]);
                publishedContentToStore.DocumentTypeAlias = attributesNameToValue["nodeTypeAlias"];
                //now we need to extract the custom properties

                foreach (System.Xml.XmlNode customProperty in node.ChildNodes)
                {
                    publishedContentToStore[customProperty.Name] = customProperty.InnerText;
                }

                //add the site to the list
                listOfContent.Add(publishedContentToStore);
            }

            return listOfContent;
        }
    }
}