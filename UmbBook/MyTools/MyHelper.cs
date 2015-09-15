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
using UmbBook.TestHelpers;
using UmbBook.Interfaces;
using Umbraco.Core.Services;
using Umbraco.Core.Persistence;


namespace UmbBook.MyTools
{
    /// <summary>
    /// Can I do this? 
    /// </summary>
    public class MyHelper : IMyHelper
    {
        private readonly UmbracoContext _umbracoContext;
        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IMediaService _mediaService;

        private readonly IRelationService _relationService;
        private readonly Database _database;


        ///Constructors needed for testability and DI
        public MyHelper(UmbracoContext _umbracoContext,
            IContentService _contentService,
            IMemberService _memberService,
            IMediaService _mediaService,
            IRelationService _relationServices,
            Database _database)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._mediaService = _mediaService;
            this._relationService = _relationServices;
            this._database = _database;
            this._umbracoContext = _umbracoContext;
        }


        public int getBrowsingUserId()
        {
            var user = HttpContext.Current.User.Identity;
            if (user != null)
            {
                if (user.IsAuthenticated)
                {
                    var browsingUser = _memberService.GetByUsername(user.Name);

                    return browsingUser.Id;
                }
            }
            return 0;
        }
     

        /// <summary>
        /// Returns a FeedsListModel with all the feed of the input userId, if the userId is -1, all feed is returned
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public FeedsListModel getAllFeedByUserId(int userID)
        {

            //first we need the relation type object
            IRelationType relationTypeToFetch = _relationService.GetRelationTypeByAlias("feedToUser");

            IEnumerable<IRelation> listOfRelationFeedToUser;

            //now we can create a relation list and sort it 

            if (userID >= 0)
            {
                listOfRelationFeedToUser = _relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).Where(x => x.ChildId == userID);
            }
            else
            {
                listOfRelationFeedToUser = _relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id);
            }

            //now let's create FeedListModel to store our feedlist
            FeedsListModel listOfAllFeeds = new FeedsListModel();


            //now let's looop over the relation to extract the information we need

            foreach (var item in listOfRelationFeedToUser)
            {
                FeedViewModel feedViewModelToStore = new FeedViewModel();

                //in the relation the child is the member id
                feedViewModelToStore.author = _memberService.GetById(item.ChildId);


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
            listOfAllFeeds.feeds = listOfAllFeeds.feeds.OrderByDescending(x => x.feed.CreateDate).ToList();

            return listOfAllFeeds;

        }
        /// <summary>
        /// Store the profilepicture has media and createa a relationship between the user and the picture
        /// </summary>
        /// <param name="userName">UserName of the user to store the profile picture to</param>
        /// <param name="file">HttpPostedFileBase of the profile picture</param>
        public void StoreProfilePicure(string userName, HttpPostedFileBase file)
        {
            //lets store the media
            var memberToStore = _memberService.GetByUsername(userName);

            //create the media item 
            var profileImageMediaToSTore = _mediaService.CreateMedia(memberToStore.Name, 1132, "Image");

            //save it to create a medi aId
            _mediaService.Save(profileImageMediaToSTore);

            //let umbraco take care of the file
            profileImageMediaToSTore.SetValue("umbracoFile", file);

            //save the whole thing
            _mediaService.Save(profileImageMediaToSTore);

            //meow we need to create the relation between the member and his profile picture
            var relationType = _relationService.GetRelationTypeByAlias("memberToProfileImage");
            var nRelation = new Relation(memberToStore.Id, profileImageMediaToSTore.Id, relationType);
            _relationService.Save(nRelation);
        }
    }
}