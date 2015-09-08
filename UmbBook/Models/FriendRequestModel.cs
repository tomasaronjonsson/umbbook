using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom

using Umbraco.Core.Models;


namespace UmbBook.Models
{
    
    public class FriendRequestModel
    {
  

        public IMember RequestingUser { get; set; }

        public IMember TargetUser { get; set; }

        public bool Accepted { get; set; }

    }
}