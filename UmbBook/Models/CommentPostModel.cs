using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using Umbraco.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UmbBook.Models
{
    public class CommentPostModel
    {
        [DataType(DataType.MultilineText)]
        [Required, Display(Name = "Your comment?")]
        public string commentContent { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int feedToCommentOn { get; set; }

        public CommentPostModel(int feedId)
        {
            feedToCommentOn = feedId;
        }
        public CommentPostModel()
        {

        }
  

    }
}