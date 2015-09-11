using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//custom
using Umbraco.Web.Mvc;
using UmbBook.Models;
using UmbBook.MyTools;
using Umbraco.Core.Models;
using Autofac.Core;
using UmbBook.Interfaces;
using Autofac.Integration.Mvc;
using Umbraco.Web;

namespace UmbBook.Controllers
{
    public class AcceptedFriendsFeedSurfaceController : SurfaceController
    {
        /// <summary>
        /// This class has dependency injection and is testable
        /// </summary>


        private readonly IAcceptedFriendsFeed _myService;

        private readonly UmbracoHelper _umbracoHelper;


        ///Constructors needed for testability
        public AcceptedFriendsFeedSurfaceController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
        }

        public AcceptedFriendsFeedSurfaceController(UmbracoContext umbracoContext, IAcceptedFriendsFeed theService)
            : base(umbracoContext)
        {
            _myService = theService;
        }


        public ActionResult renderAccptedFeed()
        {
            return PartialView("FeedsList", _myService.renderAccptedFeed());

        }
    }
}