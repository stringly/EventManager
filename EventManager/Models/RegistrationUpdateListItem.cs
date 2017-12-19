using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.Models
{
    public class RegistrationUpdateListItem
    {
        public int id { get; set; }
        public RegistrationStats status { get; set; }
        public bool notify { get; set; }
        //public string status { get; set; }
    }
}