using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UmbBook.Models;
using Umbraco.Core.Models;

namespace UmbBook.Controllers
{
    public class FeedPostSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

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
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(model.feedContent))
                {
                    return RedirectToCurrentUmbracoPage();
                }

                var contentService = Services.ContentService;

                int userId = Services.MemberService.GetByUsername(User.Identity.Name).Id;

                var feed = contentService.CreateContent(
                    "feed",
                    1084,
                    "feed", userId);

                feed.SetValue("feedContent", model.feedContent);

                contentService.SaveAndPublishWithStatus(feed);

                var relationServices = Services.RelationService;

                var relationType = relationServices.GetRelationTypeByAlias("feedToUser");
                var nRelation = new Relation(feed.Id, userId, relationType);
                relationServices.Save(nRelation);
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}
