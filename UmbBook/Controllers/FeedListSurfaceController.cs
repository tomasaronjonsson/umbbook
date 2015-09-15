using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbBook.Models;
using Umbraco.Core.Models;
using UmbBook.MyTools;
using Umbraco.Web;
using Umbraco.Core.Services;
using UmbBook.Interfaces;

namespace UmbBook.Controllers
{
    public class FeedListSurfaceController : SurfaceController
    {

        private readonly IMyHelper _myHelper;

        //Constructors needed for testability and DI
        public FeedListSurfaceController(UmbracoContext umbracoContext,
            IMyHelper _myHelper)
            : base(umbracoContext)
        {
            this._myHelper = _myHelper;
        }
        //here we utilize the myhelper class for the controller, making him slimmer ...<insert joke here>
        public ActionResult RenderFeedListByUserId(int userIdToView)
        {
            return PartialView("FeedsList", _myHelper.getAllFeedByUserId(userIdToView));

        }

        public ActionResult RenderFeedListAll()
        {
            return PartialView("FeedsList", _myHelper.getAllFeedByUserId(-1));

        }



    }
}