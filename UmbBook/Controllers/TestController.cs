using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace UmbBook.Controllers
{
    public class TestController : UmbracoApiController
    {
        private readonly IMemberService _memberService;

        ///Constructors needed for testability and DI
        public TestController(UmbracoContext umbracoContext, IMemberService _memberService)
            : base(umbracoContext)
        {
            this._memberService = _memberService;
        }

        public object GetAllUsers()
        {
            return _memberService.GetAllMembers().Select(x => new { x.Name, x.CreateDate, x.Email });
        }
    }
}