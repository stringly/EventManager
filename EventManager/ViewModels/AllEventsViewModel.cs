using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class EventsViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalHours { get; set; }
        public string Description { get; set; }
        public int AvailableStaff { get; set; }
        public string EventOwner { get; set; }
        public string EventOwnerID { get; set; }
        public string TotalCurrentRegistrations { get; set; }
    }
}