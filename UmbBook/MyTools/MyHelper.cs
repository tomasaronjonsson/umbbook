using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using UmbBook.Models;
//custom
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using System.Web.Security;
using Umbraco.Web;


namespace UmbBook.MyTools
{
    /// <summary>
    /// Can I do this? 
    /// </summary>
    public class MyHelper
    {

        public int getBrowsingUserId()
        {

            var memberService = ApplicationContext.Current.Services.MemberService;
            var user = HttpContext.Current.User.Identity;
            if (user != null)
            {
                if (user.IsAuthenticated)
                {
                    var browsingUser = memberService.GetByUsername(user.Name);

                    return browsingUser.Id;
                }
            }
            return 0;
        }
        /// <summary>
        /// Returns a FriendRequestsViewModel with a list of all accepted friends for the current browsing user
        /// </summary>
        /// <returns></returns>
        public FriendRequestsViewModel acceptedFriendsToViewModel()
        {

            //get the database object
            var database = ApplicationContext.Current.DatabaseContext.Database;


            //get the user id who is browsing
            int userId = getBrowsingUserId();

            //get all the friend request which involve him and a status of accepted
            var acceptedFriends = database.Query<UmbBook.pocos.FriendRequest>("SELECT * FROM FriendRequests WHERE RequestingUserId =@0 AND accepted=@1 OR TargetUserId = @0 AND accepted=@1", userId.ToString(), "true");


            FriendRequestsViewModel acceptedFriendsToViewModel = new FriendRequestsViewModel();
            var memberService = ApplicationContext.Current.Services.MemberService;

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
                acceptedFriendsToViewModel.friendRequests.Add(memberService.GetById(userIdToAdd));
            }
            return acceptedFriendsToViewModel;
        }


        /// <summary>
        /// Returns a FeedsListModel with all the feed of the input userId
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public FeedsListModel getAllFeedByUserId(int userID)
        {

            var relationService = ApplicationContext.Current.Services.RelationService;
            var memberService = ApplicationContext.Current.Services.MemberService;


            //first we need the relation type object
            IRelationType relationTypeToFetch = relationService.GetRelationTypeByAlias("feedToUser");


            //now we can create a relation list and sort it by createdate
            var listOfRelationFeedToUser = relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).Where(x => x.ChildId == userID);


            //now let's create FeedListModel to store our feedlist
            FeedsListModel listOfAllFeeds = new FeedsListModel();


            //now let's looop over the relation to extract the information we need

            foreach (var item in listOfRelationFeedToUser)
            {
                FeedViewModel feedViewModelToStore = new FeedViewModel();

                //in the relation the child is the member id
                feedViewModelToStore.author = memberService.GetById(item.ChildId);


                //and the parent is the feed 

                //we are going to be using the ipublished version here for the front end
                var umbHelper = new UmbracoHelper(UmbracoContext.Current);
                feedViewModelToStore.feed = umbHelper.TypedContent(item.ParentId);

                //let's make sure neither is null , else we will not add the feedviewmodel to our collection
                if (feedViewModelToStore.author != null && feedViewModelToStore.feed != null)
                {
                    listOfAllFeeds.feeds.Add(feedViewModelToStore);
                }

            }

            return listOfAllFeeds;

        }

    }
}