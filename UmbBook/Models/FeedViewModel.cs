using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using Umbraco.Core.Models;

namespace UmbBook.Models
{
    public class FeedViewModel
    {
        public IPublishedContent feed { get; set; }
        public IMember author { get; set; }

    }
}