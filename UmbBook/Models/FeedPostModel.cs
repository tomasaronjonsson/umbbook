using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmbBook.Models
{
    public class FeedPostModel
    {
        [DataType(DataType.MultilineText)]
        [Required, Display(Name = "What is on your mind?")]
        public string feedContent { get; set; }

    }
}
