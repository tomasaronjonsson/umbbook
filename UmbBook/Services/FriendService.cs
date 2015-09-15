using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using UmbBook.Interfaces;
using UmbBook.Models;
using UmbBook.MyTools;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace UmbBook.Services
{
    public class FriendService : IFriendService
    {
        private readonly IMyHelper _myHelper;
        private readonly Database _database;
        private readonly IMemberService _memberService;


        public FriendService(
            IMyHelper _myHelper,
            Database _database,
            IMemberService _memberService)
        {
            this._myHelper = _myHelper;
            this._database = _database;
            this._memberService = _memberService;
        }

        public FeedsListModel renderAccptedFeed()
        {

            //now we got a list of all accepted friends for the logged in user
            FriendRequestsViewModel acceptedFriends = acceptedFriendsToViewModel();

            //lets create a feeds list model to return
            FeedsListModel feedsListToReturn = new FeedsListModel();

            //now createa a collection of feeds from all the users
            foreach (var a in acceptedFriends.friendRequests)
            {
                var allFeedFromUser = _myHelper.getAllFeedByUserId(a.Id);

                feedsListToReturn.feeds.AddRange(allFeedFromUser.feeds);

            }
            //lets fix the order of things
            feedsListToReturn.feeds = feedsListToReturn.feeds.OrderByDescending(x => x.feed.CreateDate).ToList();

            return feedsListToReturn;
        }

        /// <summary>
        /// Returns a FriendRequestsViewModel with a list of all accepted friends for the current browsing user
        /// </summary>
        /// <returns></returns>
        public FriendRequestsViewModel acceptedFriendsToViewModel()
        {
            //get the user id who is browsing
            int userId = _myHelper.getBrowsingUserId();

            //get all the friend request which involve him and a status of accepted
            var acceptedFriends = _database.Query<UmbBook.pocos.FriendRequest>("SELECT * FROM FriendRequests WHERE RequestingUserId =@0 AND accepted=@1 OR TargetUserId = @0 AND accepted=@1", userId.ToString(), "true");

            FriendRequestsViewModel acceptedFriendsToViewModel = new FriendRequestsViewModel();

            //let's get the imember objects for each and store it in the 
            foreach (var item in acceptedFriends)
            {
                //because we got 2 ids , but are just interested in either one of them (not the user himself
                int userIdToAdd;

                if (item.RequestingUserId == userId)
                {
                    userIdToAdd = item.TargetUserId;
                }
                else
                {
                    userIdToAdd = item.RequestingUserId;
                }
                acceptedFriendsToViewModel.friendRequests.Add(_memberService.GetById(userIdToAdd));
            }
            return acceptedFriendsToViewModel;
        }



        public FriendRequestsViewModel RenderFriendRequests()
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

            return incomcingFriendRequests;
        }


        public string RequestFriend(string userId)
        {
            //collected the values
            UmbBook.pocos.FriendRequest pocoToInsert = new UmbBook.pocos.FriendRequest();
            //check if there's some input
            if (userId == null)
            {
                return "trying to break me? or befriending yourself? useless!";

            }
            //read in the target user
            pocoToInsert.TargetUserId = Int32.Parse(userId);

            //check if the request is okey
            if (pocoToInsert.TargetUserId == 0)
            {
                return "Friend not found, feeling alone?";
            }

            int browsingUserId = _myHelper.getBrowsingUserId();

            //lets see if the requesting friend is the same as the target
            if (pocoToInsert.TargetUserId == browsingUserId)
            {
                return "Trying to befriend yourself? tiss tiss sad....";
            }


            pocoToInsert.RequestingUserId = browsingUserId;

            //remember to set this as false, dunno
            pocoToInsert.Accepted = false;

            _database.Insert(pocoToInsert);

            return "Friend request sent!";

        }


        public void AcceptFriend(int userId)
        {
            _database.Execute("UPDATE FriendRequests set accepted = @0 WHERE targetUserId = @1 AND requestingUserId = @2", "true", _myHelper.getBrowsingUserId(), userId);
        }
    }
}