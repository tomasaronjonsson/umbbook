using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//customg using
using Umbraco.Core.Models;


namespace UmbBook.Models
{
    public class CommentViewModel
    {
        public IMember author { get; set; }
        public IPublishedContent commentContent { get; set; }


    }
}