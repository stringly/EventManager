//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventManager
{
    using System;
    
    public partial class ALL_EVENTS_VIEW_Result
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> TotalHours { get; set; }
        public string Description { get; set; }
        public Nullable<int> AvailableStaff { get; set; }
        public string EventOwner { get; set; }
        public int EventOwnerID { get; set; }
        public string EventOwnerEmail { get; set; }
        public Nullable<int> TotalCurrentRegistrations { get; set; }
    }
}
