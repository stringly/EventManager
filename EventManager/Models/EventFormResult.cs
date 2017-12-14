using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManager.Helpers;

namespace EventManager.Models
{
    public class EventFormResult
    {
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string MinStaff { get; set; }
        public string MaxStaff { get; set; }
        public string FundCenter { get; set; }
        public int EnteredBy { get; set; }
        public string DisplayColor { get; set; }
        public string repeatType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int[] dow { get; set; }
        public string repeatCount { get; set; }
    }


}
