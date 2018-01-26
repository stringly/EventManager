using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class EventRegistrationViewModel
    {
        public List<AvailableEvents> ViewUserAvailableEvents { get; set; }
        public List<UserRegistrations> ViewUserRegistrations { get; set; }
    }

    public class UserRegistrations
    {
        public int RegistrationID { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string EventDescription { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public RegistrationStatus Status { get; set; }
    }
    public class AvailableEvents
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int MaxStaff { get; set; }
        public int MinStaff { get; set; }
        public string FundCenter { get; set; }
        public int EnteredBy { get; set; }
    }
}