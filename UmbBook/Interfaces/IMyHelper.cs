using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbBook.Models;

namespace UmbBook.Interfaces
{
    public interface IMyHelper
    {
        int getBrowsingUserId();

        FriendRequestsViewModel acceptedFriendsToViewModel();

        FeedsListModel getAllFeedByUserId(int userID);

        void StoreProfilePicure(string userName, System.Web.HttpPostedFileBase file);
    }
}
