using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmbBook.Models
{
    public class MemberLoginModel
    {
        [Required, Display(Name = "Enter your user name")]
        public string userName { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Remember me")]
        public bool rememberMe { get; set; }

    }
}