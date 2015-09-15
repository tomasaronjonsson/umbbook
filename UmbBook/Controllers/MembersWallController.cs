using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UmbBook.Interfaces;
using UmbBook.Models;
using UmbBook.MyTools;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace UmbBook.Controllers
{
    public class MembersWallController : Umbraco.Web.Mvc.RenderMvcController
    {
        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IRelationService _relationService;
        private readonly Database _database;
        private readonly IMyHelper _myHelper;

        //Constructors needed for testability and DI
        public MembersWallController(UmbracoContext umbracoContext,
            IContentService _contentService,
            IMemberService _memberService,
            IRelationService _relationService,
            Database _database,
            IMyHelper _myHelper)
            : base(umbracoContext)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._relationService = _relationService;
            this._database = _database;
            this._myHelper = _myHelper;
        }

        public override System.Web.Mvc.ActionResult Index(Umbraco.Web.Models.RenderModel model)
        {

            MembersWallModel membersWall = new MembersWallModel();

            if (User.Identity.IsAuthenticated)
            {
                //lets get the user id either from the query or the current user
                int userIdToView;
                int userCurrentUser = _myHelper.getBrowsingUserId();
                if (!Int32.TryParse(Request.Params.Get("id"), out userIdToView) || userIdToView == 0)
                {
                    userIdToView = userCurrentUser;
                }

                //store the user we are going to use
                var user = _memberService.GetById(userIdToView);
                //flag it if the browsing user is looking at his own profile
                if (userCurrentUser == userIdToView)
                {
                    membersWall.isThisHisOwnWall = true;
                }

                //store the basic info                
                membersWall.owner = user;

                //lets add the profile image to the user

                //first we need the relation type
                IRelationType relationTypeToFetch = _relationService.GetRelationTypeByAlias("memberToProfileImage");

                //lets try to find a relationship between the member and some profile picture
                var memberToProfileImageRelation = _relationService.GetAllRelationsByRelationType(relationTypeToFetch.Id).Where(x => x.ParentId == membersWall.owner.Id);

                //get all the pictures
                var profileImageToUseRelation = memberToProfileImageRelation.Select(x => Umbraco.TypedMedia(x.ChildId));

                //select the newest picture and get the url for it
                var profileImageToUseMedia = profileImageToUseRelation.OrderByDescending(x => x.CreateDate);

                if (profileImageToUseMedia.Count() > 0)
                {
                    if (profileImageToUseMedia.First() != null)
                    {
                        membersWall.profileImage = profileImageToUseMedia.First().Url;
                    }
                }

            }
            else
            {
                return PartialView("MemberLogin");
            }

            return CurrentTemplate(membersWall);
        }

    }
}