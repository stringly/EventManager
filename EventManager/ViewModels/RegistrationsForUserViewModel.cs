using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManager.ViewModels
{
    public class RegistrationsForUserViewModel
    {
        public int RegistrationID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public double TotalHours { get; set; }
        public string EventOwner { get; set; }
        public string EventOwnerEmail { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public RegistrationStatus Status { get; set; }
    }
}