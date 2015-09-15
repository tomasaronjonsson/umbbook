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

        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IRelationService _relationService;
        private readonly Database _database;
        private readonly IMyHelper _myHelper;


        //Constructors needed for testability and DI
        public FriendRequestSurfaceController(UmbracoContext umbracoContext,
            IContentService _contentService,
            IMemberService _memberService,
            IRelationService _relationService,
            Database _database,
            IMyHelper _myHelper)
            : base(umbracoContext)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._relationService = _relationService;
            this._database = _database;
            this._myHelper = _myHelper;
        }

        //todo this guy is going to render the accepted friends
        public ActionResult RenderAcceptedFriends()
        {
            return PartialView("AcceptedFriends", _myHelper.acceptedFriendsToViewModel());
        }

        //this guy is going to render incoming friend requests
        public ActionResult RenderFriendRequests()
        {

            //get the user id who is browsing
            int userId = _myHelper.getBrowsingUserId();

            //get all the friend request which involve him and a status of not accepted
            var friendRequests = _database.Query<UmbBook.pocos.FriendRequest>("SELECT * FROM FriendRequests WHERE TargetUserId = @0 AND accepted = @1", userId.ToString(), "false");

            FriendRequestsViewModel incomcingFriendRequests = new FriendRequestsViewModel();

            //let's get the imember objects for each and store it in the 
            foreach (var item in friendRequests)
            {
                incomcingFriendRequests.friendRequests.Add(_memberService.GetById(item.RequestingUserId));
            }
            return PartialView("IncomingFriendRequests", incomcingFriendRequests);

        }


        //this guy is going to send friend requests seems to be sending
        [HttpPost]
        [ActionName("RequestFriend")]
        public ActionResult RequestFriend()
        {

            //collected the values
            UmbBook.pocos.FriendRequest pocoToInsert = new UmbBook.pocos.FriendRequest();
            //check if there's some input
            if (Request.Params.Get("id") == null)
            {
                TempData["Status"] = "trying to break me? or befriending yourself? useless!";
                return RedirectToCurrentUmbracoUrl();
            }
            //read in the target user
            pocoToInsert.TargetUserId = Int32.Parse(Request.Params.Get("id"));

            //check if the request is okey
            if (pocoToInsert.TargetUserId == 0)
            {
                TempData["Status"] = "Friend not found, feeling alone?";
                return RedirectToCurrentUmbracoUrl();
            }

            var browsingUser = _memberService.GetByUsername(User.Identity.Name);

            //lets see if the requesting friend is the same as the target
            if (pocoToInsert.TargetUserId == browsingUser.Id)
            {
                TempData["Status"] = "Trying to befriend yourself? tiss tiss sad....";
                return RedirectToCurrentUmbracoUrl();
            }


            pocoToInsert.RequestingUserId = browsingUser.Id;

            //remember to set this as false, dunno
            pocoToInsert.Accepted = false;

            _database.Insert(pocoToInsert);


            TempData["Status"] = "Friend request sent";

            return RedirectToCurrentUmbracoUrl();
        }


        //this guys is to accept friend request

        public ActionResult AcceptFriend(int userId)
        {
            //updating the SQL database

            _database.Execute("UPDATE FriendRequests set accepted = @0 WHERE targetUserId = @1 AND requestingUserId = @2", "true", _myHelper.getBrowsingUserId(), userId);

            return Redirect("/members-wall/");
        }



        //this one works fine
        public ActionResult flushtable()
        {
            _database.Execute("DELETE FROM FriendRequests");
            return null;
        }

    }
}