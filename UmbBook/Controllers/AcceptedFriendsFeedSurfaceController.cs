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

namespace UmbBook.Controllers
{
    public class AcceptedFriendsFeedSurfaceController : SurfaceController
    {


        private readonly IAcceptedFriendsFeed _myService;

        public AcceptedFriendsFeedSurfaceController(IAcceptedFriendsFeed theService)
        {
            _myService = theService;
        }


        public ActionResult renderAccptedFeed()
        {           
            return PartialView("FeedsList", _myService.renderAccptedFeed());

        }
    }
}