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
    using System.Collections.Generic;
    
    public partial class Registration
    {
        public int RegistrationID { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public int Status { get; set; }
    
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
