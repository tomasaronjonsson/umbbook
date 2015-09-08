using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmbBook.Models
{
    public class MemberRegisterModel
    {
        [Required, Display(Name = "Enter your user name")]
        public string userName { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string password { get; set; }

        [Required, Display(Name = "Repeat password"), DataType(DataType.Password)]
        public string repeatPassword { get; set; }

        [Required, Display(Name = "Email"),DataType(DataType.EmailAddress)]
        public String email { get; set; }

    }
}