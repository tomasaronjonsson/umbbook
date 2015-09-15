using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//custom
using Umbraco.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Umbraco.Core.Models;
using UmbBook.Models;
using UmbBook.MyTools;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Core.Persistence;
using UmbBook.Interfaces;

namespace UmbBook.Controllers
{
    public class FriendRequestSurfaceController : SurfaceController
    {

        private readonly IFriendService _friendService;

        //Constructors needed for testability and DI
        public FriendRequestSurfaceController(UmbracoContext umbracoContext,
            IFriendService _friendService)
            : base(umbracoContext)
        {
            this._friendService = _friendService;
        }

        //this guy is going to render the accepted friends
        public ActionResult RenderAcceptedFriends()
        {
            return PartialView("AcceptedFriends", _friendService.acceptedFriendsToViewModel());
        }

        //this guy is going to render incoming friend requests
        public ActionResult RenderFriendRequests()
        {
            return PartialView("IncomingFriendRequests", _friendService.RenderFriendRequests());

        }

        //this guy is going to send friend requests seems to be sending
        [HttpPost]
        [ActionName("RequestFriend")]
        public ActionResult RequestFriend()
        {
            TempData["Status"] = _friendService.RequestFriend(Request.Params.Get("id"));

            return RedirectToCurrentUmbracoUrl();
        }

        //this guys is to accept friend request
        public ActionResult AcceptFriend(int userId)
        {
            
            _friendService.AcceptFriend(userId);
            return Redirect("/members-wall/");
        }

    }
}