using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public int Rank { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IDNumber { get; set; }
        public string Email { get; set; }
        public string PayrollID { get; set; }
        public string ContactNumber { get; set; }
        public string LDAPName { get; set; }
    }
}