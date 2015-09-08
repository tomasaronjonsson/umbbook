using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using Umbraco.Core.Models;

namespace UmbBook.Models
{
    public class MembersWallModel
    {
        public bool isThisHisOwnWall = false;
        public IMember owner;
        public List<FeedViewModel> membersFeed { get; set; }

        public string profileImage;

        public MembersWallModel()
        {
            membersFeed = new List<FeedViewModel>();
        }
    }
}