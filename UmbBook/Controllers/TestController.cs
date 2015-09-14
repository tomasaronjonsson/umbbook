using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace UmbBook.Controllers
{
    public class TestController : UmbracoApiController
    {

        public object GetAllUsers()
        {
            var memberService = ApplicationContext.Services.MemberService;


            return memberService.GetAllMembers().Select(x => new { x.Name, x.CreateDate, x.Email });
        }
    }
}