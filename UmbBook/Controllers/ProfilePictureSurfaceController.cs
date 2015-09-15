using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbBook.Models;
using System.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbBook.Interfaces;

namespace UmbBook.Controllers
{
    public class ProfilePictureSurfaceController : SurfaceController
    {

        private readonly IContentService _contentService;
        private readonly IMemberService _memberService;
        private readonly IMediaService _mediaService;
        private readonly IMyHelper _myHelper;
        private readonly IRelationService _relationService;

        //Constructors needed for testability and DI
        public ProfilePictureSurfaceController(UmbracoContext umbracoContext,
            IContentService _contentService,
            IMemberService _memberService,
            IMediaService _mediaService,
            IMyHelper _myHelper,
            IRelationService _relationService)
            : base(umbracoContext)
        {
            this._contentService = _contentService;
            this._memberService = _memberService;
            this._mediaService = _mediaService;
            this._relationService = _relationService;
            this._myHelper = _myHelper;
        }


        [HttpPost]
        [ActionName("ProfileImagePost")]
        public ActionResult ProfileImagePost()
        {
            //check if the user is logged in
            if (User.Identity.IsAuthenticated)
            {
                //where any files uploaded?
                if (Request.Files.Count > 0)
                {
                    //lets check on that file
                    var profileImageFile = Request.Files[0];

                    if (profileImageFile != null && profileImageFile.ContentLength > 0)
                    {

                        //lets store the media
                        var memberToStore = _memberService.GetByUsername(User.Identity.Name);

                        //create the media item 
                        var profileImageMediaToSTore = _mediaService.CreateMedia(memberToStore.Name, 1132, "Image");

                        //save it to create a medi aId
                        _mediaService.Save(profileImageMediaToSTore);

                        //let umbraco take care of the file
                        profileImageMediaToSTore.SetValue("umbracoFile", Request.Files[0]);

                        //save the whole thing
                        _mediaService.Save(profileImageMediaToSTore);

                        //meow we need to create the relation between the member and his profile picture
                        var relationType = _relationService.GetRelationTypeByAlias("memberToProfileImage");
                        var nRelation = new Relation(memberToStore.Id, profileImageMediaToSTore.Id, relationType);
                        _relationService.Save(nRelation);
                    }

                }

            }



            return RedirectToCurrentUmbracoPage();
        }
    }

}