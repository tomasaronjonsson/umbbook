using System;
using System.Collections.Generic;
using System.Linq;
//custom
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UmbBook.Models;

namespace UmbBook.Controllers
{
    public class MemberLoginSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

        
        //members to log out
        [HttpGet]
        [ActionName("MemberLogout")]
        public ActionResult MemberLogout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        //members to login
        [HttpPost]
        [ActionName("MemberLogin")]
        public ActionResult MemberLoginPost(MemberLoginModel model)
        {
            if (Membership.ValidateUser(model.userName, model.password))
            {
                FormsAuthentication.SetAuthCookie(model.userName, model.rememberMe);
                return RedirectToUmbracoPage(1103);

            }
            else
            {
                TempData["Status"] = "Invalid username or password";
                return RedirectToCurrentUmbracoPage();
            }

        }
    }
}