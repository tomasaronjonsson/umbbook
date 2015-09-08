using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using Umbraco.Core.Models;

namespace UmbBook.Models
{
    public class CommentsListModel
    {
        public List<CommentViewModel> comments { get; set; }

        public int feedToViewCommentsFrom { get; set; }
        
        public CommentsListModel()
        {
            comments = new List<CommentViewModel>();
        }

    }
}