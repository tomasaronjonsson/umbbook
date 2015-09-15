using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//customg
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbBook.Models;
using Umbraco.Core.Models;
namespace UmbBook.Controllers
{
    public class CommentSurfaceController : SurfaceController
    {

        



        [HttpPost]
        [ActionName("CommentPost")]
        public ActionResult CommentPost(CommentPostModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(model.commentContent))
                {
                    return RedirectToCurrentUmbracoPage();
                }
                if (model.feedToCommentOn == 0)
                {
                    return RedirectToCurrentUmbracoPage();
                }
                var contentService = Services.ContentService;

                int userId = Services.MemberService.GetByUsername(User.Identity.Name).Id;


                //add content
                var commentToStore = contentService.CreateContent(
                    "comment",
                    model.feedToCommentOn,
                    "comment", userId);

                commentToStore.SetValue("commentContent", model.commentContent);

                contentService.SaveAndPublishWithStatus(commentToStore);

                var relationServices = Services.RelationService;
                //create the relation betwen the feed and the member
                var relationType = relationServices.GetRelationTypeByAlias("commentToMember");
                var nRelation = new Relation(commentToStore.Id, userId, relationType);
                relationServices.Save(nRelation);
            }

            return RedirectToCurrentUmbracoPage();
        }

        public ActionResult RenderComments(int feedToViewCommentsFrom)
        {
            CommentsListModel listOfCommentsToReturn = new CommentsListModel();

            //store the current page so we can use it later when we are posting comments on the site
            listOfCommentsToReturn.feedToViewCommentsFrom = feedToViewCommentsFrom;

            //lets get all the relation item for this page
            var relationService = ApplicationContext.Services.RelationService;

            //first we need the relation type object
            IRelationType relationTypeToFetch = relationService.GetRelationTypeByAlias("commentToMember");

            //lets get a list of comments on this page
            var contentService = ApplicationContext.Services.ContentService;
            var allChildContentOfTheFeed = contentService.GetChildren(feedToViewCommentsFrom).Select(x => x.Id);
            


            //now we can create a relation list and sort it by createdate but only the ones that are in allChildContentOfTheFeed
            var listOfRelationCommentToMember = relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).Where(x => allChildContentOfTheFeed.Contains(x.ParentId)).OrderByDescending(x => x.CreateDate);


            var memberService = ApplicationContext.Services.MemberService;

            foreach (var item in listOfRelationCommentToMember)
            {
                
                CommentViewModel commentViewModelToAdd = new CommentViewModel();

                //get the member to store in the author
                commentViewModelToAdd.author = memberService.GetById(item.ChildId);

                //get the content to store as the comment
                commentViewModelToAdd.commentContent = Umbraco.TypedContent(item.ParentId);

                
                //checked if either is null else we dont' add them

                if (commentViewModelToAdd.author != null && commentViewModelToAdd.commentContent != null)
                {
                    listOfCommentsToReturn.comments.Add(commentViewModelToAdd);
                }

            }

            return PartialView("CommentsList", listOfCommentsToReturn);
        }
    }

}