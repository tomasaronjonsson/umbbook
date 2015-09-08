using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//custom
using Umbraco.Core.Models;
namespace UmbBook.Models
{
    public class FriendRequestsViewModel
    {
        public List<IMember> friendRequests { get; set; }

         
        public FriendRequestsViewModel()
        {
            friendRequests = new List<IMember>();
        }
    }
}