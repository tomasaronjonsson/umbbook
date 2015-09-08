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

namespace UmbBook.Controllers
{
    public class ProfilePictureSurfaceController : SurfaceController
    {

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

                        var memberService = ApplicationContext.Services.MemberService;

                        var mediaService = ApplicationContext.Services.MediaService;

                        var relationService = ApplicationContext.Services.RelationService;

                        //lets store the media
                        var memberToStore = memberService.GetByUsername(User.Identity.Name);

                        //create the media item 
                        var profileImageMediaToSTore = mediaService.CreateMedia(memberToStore.Name, 1132, "Image");

                        //save it to create a medi aId
                        mediaService.Save(profileImageMediaToSTore);


                        //let umbraco take care of the file
                        profileImageMediaToSTore.SetValue("umbracoFile",Request.Files[0]);

                        

                        //save the whole thing
                        mediaService.Save(profileImageMediaToSTore);

                        

                        //meow we need to create the relation between the member and his profile picture

                        var relationType = relationService.GetRelationTypeByAlias("memberToProfileImage");

                        var nRelation = new Relation(memberToStore.Id, profileImageMediaToSTore.Id, relationType);
                        
                        relationService.Save(nRelation);


                        
                    }

                }

            }



            return RedirectToCurrentUmbracoPage();
        }
    }

}