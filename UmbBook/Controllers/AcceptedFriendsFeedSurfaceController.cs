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
        private readonly IFriendService _friendHelper;

        ///Constructors needed for testability and DI
             public AcceptedFriendsFeedSurfaceController(UmbracoContext umbracoContext, IFriendService theService)
            : base(umbracoContext)
        {
            _friendHelper = theService;
        }


        public ActionResult renderAccptedFeed()
        {
            return PartialView("FeedsList", _friendHelper.renderAccptedFeed());

        }
    }
}