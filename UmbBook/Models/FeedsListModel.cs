using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace UmbBook.Models
{
    public class FeedsListModel
    {

        public List<FeedViewModel> feeds { get; set; }

        public FeedsListModel()
        {
            feeds = new List<FeedViewModel>();
        }
      
    }
}