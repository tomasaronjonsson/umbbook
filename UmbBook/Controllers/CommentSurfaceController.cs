using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//customg
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using UmbBook.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core.Services;
namespace UmbBook.Controllers
{
    public class CommentSurfaceController : SurfaceController
    {
        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IRelationService _relationService;


        ///Constructors needed for testability and DI
        public CommentSurfaceController(UmbracoContext umbracoContext, 
            IContentService _contentService, 
            IMemberService _memberService, 
            IRelationService _relationServices)
            : base(umbracoContext)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._relationService = _relationServices;
        }



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

                int userId = _memberService.GetByUsername(User.Identity.Name).Id;


                //add content
                var commentToStore = _contentService.CreateContent(
                    "comment",
                    model.feedToCommentOn,
                    "comment", userId);

                commentToStore.SetValue("commentContent", model.commentContent);

                _contentService.SaveAndPublishWithStatus(commentToStore);

                //create the relation betwen the feed and the member
                var relationType = _relationService.GetRelationTypeByAlias("commentToMember");
                var nRelation = new Relation(commentToStore.Id, userId, relationType);
                _relationService.Save(nRelation);
            }

            return RedirectToCurrentUmbracoPage();
        }

        public ActionResult RenderComments(int feedToViewCommentsFrom)
        {
            CommentsListModel listOfCommentsToReturn = new CommentsListModel();

            //store the current page so we can use it later when we are posting comments on the site
            listOfCommentsToReturn.feedToViewCommentsFrom = feedToViewCommentsFrom;

            //lets get all the relation item for this page

            //first we need the relation type object
            IRelationType relationTypeToFetch = _relationService.GetRelationTypeByAlias("commentToMember");

            //lets get a list of comments on this page
            var contentService = ApplicationContext.Services.ContentService;
            var allChildContentOfTheFeed = contentService.GetChildren(feedToViewCommentsFrom).Select(x => x.Id);



            //now we can create a relation list and sort it by createdate but only the ones that are in allChildContentOfTheFeed
            var listOfRelationCommentToMember = _relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).Where(x => allChildContentOfTheFeed.Contains(x.ParentId)).OrderByDescending(x => x.CreateDate);


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