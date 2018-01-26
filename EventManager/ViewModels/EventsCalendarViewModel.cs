using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    /// <summary>
    /// This is a class to populate the list of events for the calendar view
    /// It is basically a port of the EF Model, minus the virtual classes.
    /// </summary>
    public class EventsCalendarViewModel
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
    }
}