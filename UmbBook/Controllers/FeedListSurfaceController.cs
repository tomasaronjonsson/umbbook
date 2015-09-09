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

namespace UmbBook.Controllers
{
    public class FeedListSurfaceController : SurfaceController
    {
   
        /// <summary>
        /// Method to get a feed by a user name, used to view members wall or own feed use RenderFeedListAll to get all for the feed list 
        /// </summary>
        /// <param name="UserIdToView"></param>
        /// <returns></returns>
        public ActionResult RenderFeedListByUserId(int userIdToView)
        {
            //lets get a helper
            var myHelper = new MyHelper();

            //let's send the list
            return PartialView("FeedsList", myHelper.getAllFeedByUserId(userIdToView));

        }

        /// <summary>
        /// Returns a partial view with all the feeds in the system
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderFeedListAll()
        {
            FeedsListModel feedListToReturn = new FeedsListModel();

            var contentSerivce = ApplicationContext.Services.ContentService;
            var relationService = ApplicationContext.Services.RelationService;
            var memberService = ApplicationContext.Services.MemberService;

            //lets get all the relation of type feedtouser

            //first we need the relation type object
            IRelationType relationTypeToFetch = relationService.GetRelationTypeByAlias("feedToUser");


            //now we can create a relation list and sort it by createdate
            var listOfRelationFeedToUser = relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).OrderByDescending(x => x.CreateDate);


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
                feedViewModelToStore.feed = Umbraco.TypedContent(item.ParentId);



                //let's make sure neither is null , else we will not add the feedviewmodel to our collection
                if (feedViewModelToStore.author != null && feedViewModelToStore.feed != null)
                {
                    listOfAllFeeds.feeds.Add(feedViewModelToStore);
                }

            }

            return PartialView("FeedsList", listOfAllFeeds);

        }



    }
}