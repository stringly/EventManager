using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class MyEventsViewModel
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
        public string DisplayColor { get; set; }
        public int TotalHours { get; set; }
        public int AvailableStaff { get; set; }
        public int TotalCurrentRegistrations { get; set; }
    }
}