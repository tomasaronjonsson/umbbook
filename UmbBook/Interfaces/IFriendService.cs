using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//custom
using UmbBook.Models;
namespace UmbBook.Interfaces
{
    public interface IFriendService
    {

       FeedsListModel renderAccptedFeed();

       FriendRequestsViewModel acceptedFriendsToViewModel();

       FriendRequestsViewModel RenderFriendRequests();

       String RequestFriend(string userId);

       void AcceptFriend(int userId);
    }
}
