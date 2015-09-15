using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UmbBook.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace UmbBook.Controllers
{
    public class FeedPostSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IRelationService _relationService;


        //Constructors needed for testability and DI
        public FeedPostSurfaceController(UmbracoContext umbracoContext,
            IContentService _contentService,
            IMemberService _memberService,
            IRelationService _relationService)
            : base(umbracoContext)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._relationService = _relationService;
        }


        [HttpGet]
        [ActionName("FeedPost")]
        public ActionResult FeedPost()
        {
            return PartialView("FeedPost", new FeedPostModel());
        }



        [HttpPost]
        [ActionName("FeedPost")]
        public ActionResult FeedPost(FeedPostModel model)
        {
            //check if user is logged in
            if (User.Identity.IsAuthenticated)
            {
                //check if the feed is okay
                if (String.IsNullOrEmpty(model.feedContent))
                {
                    return RedirectToCurrentUmbracoPage();
                }

                //get the username of the poster
                int userId = _memberService.GetByUsername(User.Identity.Name).Id;

                //create a new content 
                var feed = _contentService.CreateContent(
                    "feed",
                    1084,
                    "feed", userId);
                //store the value
                feed.SetValue("feedContent", model.feedContent);
                //published it and save with the content service
                _contentService.SaveAndPublishWithStatus(feed);

                //create a relationship between the posting user and the feed, and save it
                var relationType = _relationService.GetRelationTypeByAlias("feedToUser");
                var nRelation = new Relation(feed.Id, userId, relationType);
                _relationService.Save(nRelation);
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}
