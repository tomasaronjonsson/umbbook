using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using UmbBook.Interfaces;
using UmbBook.Models;
using UmbBook.MyTools;

namespace UmbBook.Services
{
    public class AcceptedFriendsFeedService : IAcceptedFriendsFeed
    {
        private readonly MyHelper _myHelper;

        
        public AcceptedFriendsFeedService(MyHelper _myHelper)
        {
            this._myHelper = _myHelper;
        }
        
        public FeedsListModel renderAccptedFeed()
        {

            //now we got a list of all accepted friends for the logged in user
            FriendRequestsViewModel acceptedFriends = _myHelper.acceptedFriendsToViewModel();

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
    }
}