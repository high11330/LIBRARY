using Library.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models.ViewModel
{
    public class User_VM : User
    {
        public string Action { get; set; }
        public string PasswordConfirm { get; set; }
    }
}