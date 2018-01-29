using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class ManageEventViewModel
    {
        public Event Event { get; set; }
        public List<RegistrationDetail> CurrentRegistrations { get; set; }
    }

    public class RegistrationDetail
    {
        public Registration Registration { get; set; }
        public UserDetails UserDetails { get; set; }
    }

    public class UserDetails
    {
        public User User { get; set; }
        public int TotalRegistrations { get; set; }
        public double TotalHoursLast30Days { get; set; }
        public double TotalHoursLast60Days { get; set; }
        public double TotalHoursLast90Days { get; set; }
        public LTRank UserRank { get; set; }

    }
}