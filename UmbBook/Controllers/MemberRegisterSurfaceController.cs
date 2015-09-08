using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UmbBook.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;


namespace UmbBook.Controllers
{
    public class MemberRegisterSurfaceController : SurfaceController
    {
        [HttpGet]
        [ActionName("MemberRegister")]
        public ActionResult MemberRegister()
        {
            return PartialView("MemberRegister", new MemberRegisterModel());
        }

        [HttpPost]
        [ActionName("MemberRegister")]
        public ActionResult MemberRegister(MemberRegisterModel model)
        {

            if (model.password != model.repeatPassword)
            {
                TempData["Status"] = "Passwords don't match";
            }
            else if (Membership.FindUsersByEmail(model.email).Count > 0)
            {
                TempData["Status"] = "Email already registered.";
            }
            else if (Services.MemberService.Exists(model.userName))
            {
                TempData["Status"] = "Username already taken";
            }
            else
            {
                IMemberType mt = Services.MemberTypeService.Get("sitemember");
                IMember member = Services.MemberService.CreateMemberWithIdentity(model.userName, model.email, model.userName, mt);
                Services.MemberService.SavePassword(member,model.password);

                TempData["Status"] = "You have been registered!";
            }
            return CurrentUmbracoPage();
            

        }

    }
}