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
        private MyHelper _MyHelper { get; set; }

        
        public FeedsListModel renderAccptedFeed()
        {
            //get our helper
            var myHelper = new MyHelper();

            //now we got a list of all accepted friends for the logged in user
            FriendRequestsViewModel acceptedFriends = myHelper.acceptedFriendsToViewModel();

            //lets create a feeds list model to return
            FeedsListModel feedsListToReturn = new FeedsListModel();

            //now createa a collection of feeds from all the users
            foreach (var a in acceptedFriends.friendRequests)
            {
                var allFeedFromUser = myHelper.getAllFeedByUserId(a.Id);

                feedsListToReturn.feeds.AddRange(allFeedFromUser.feeds);

            }
            //lets fix the order of things
            feedsListToReturn.feeds = feedsListToReturn.feeds.OrderByDescending(x => x.feed.CreateDate).ToList();

            return feedsListToReturn;
        }
    }
}