using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//custom
using UmbBook.Models;
namespace UmbBook.Interfaces
{
    public interface IAcceptedFriendsFeed
    {

       FeedsListModel renderAccptedFeed();
    }
}
