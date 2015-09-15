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

        private readonly IMyHelper _myHelper;

        //Constructors needed for testability and DI
        public ProfilePictureSurfaceController(UmbracoContext umbracoContext,
            IMyHelper _myHelper)
            : base(umbracoContext)
        {
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
                    if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                    {
                        //all good to go 
                        _myHelper.StoreProfilePicure(User.Identity.Name, Request.Files[0]);

                    }

                }

            }



            return RedirectToCurrentUmbracoPage();
        }
    }

}