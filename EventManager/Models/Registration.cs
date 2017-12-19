using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManager.Helpers;
using System.Configuration;
using System.Diagnostics;

namespace EventManager
{
    public partial class Registration
    {
        public void NotifyRegistrationStatusChange()
        {
            //Email            
            EmailHelper e = new EmailHelper();
            e.AddToAddress(User.Email);
            e.Subject = "Registration Status Notification";
            //create body from template here
            e.RegistrationBody("", Event.EventName, Event.StartTime, Status, Event.User.Email);
            e.SendMail();
        }
    }
}