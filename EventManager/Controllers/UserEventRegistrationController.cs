﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.ViewModels;
using EventManager.Helpers;

namespace EventManager.Controllers
{
    public class UserEventRegistrationController : Controller
    {
        private EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities();
        private User user = new EventManager.User();

        // GET: UserEventRegistration
        public ActionResult Index()
        {
            string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
            user = _dc.Users.Where(u => u.LDAPName == nameWithoutDomain).FirstOrDefault();
            EventRegistrationViewModel ervm = new EventRegistrationViewModel();
            ervm.ViewUserAvailableEvents = GetUserAvailableEvents();
            ervm.ViewUserRegistrations = GetUserRegistrations();


            return View(ervm);
        }

        public List<AvailableEvents> GetUserAvailableEvents()
        {
            List<AvailableEvents> availableEventsForUser = new List<AvailableEvents>();
            var eventList = _dc.Events.Where(e =>
                                !_dc.Registrations.Any(r => r.UserID == user.UserId && r.EventID == e.EventID)
                                && e.StartTime >= DateTime.Now)
                    .OrderBy(e => e.StartTime)
                    .ToList();
            foreach (var item in eventList)
            {
                AvailableEvents ae = new AvailableEvents();
                ae.EventID = item.EventID;
                ae.EventName = item.EventName;
                ae.StartTime = item.StartTime;
                ae.EndTime = item.EndTime;
                ae.Description = item.Description;
                ae.MaxStaff = item.MaxStaff;
                ae.MinStaff = item.MinStaff;
                ae.FundCenter = item.FundCenter;
                ae.EnteredBy = item.EnteredBy;
                availableEventsForUser.Add(ae);
            }
            return availableEventsForUser;
        }

        public List<UserRegistrations> GetUserRegistrations()
        {
            List<UserRegistrations> userRegistrations = new List<UserRegistrations>();
            var registrationList = _dc.Registrations.Where(e => e.UserID == user.UserId 
                                        && e.Event.StartTime >= DateTime.Now 
                                        && (e.Status != RegistrationStats.Deleted 
                                        && e.Status != RegistrationStats.Declined 
                                        && e.Status != RegistrationStats.NoShow)).ToList();
            foreach (var item in registrationList)
            {
                UserRegistrations ur = new UserRegistrations();
                ur.RegistrationID = item.RegistrationID;
                ur.UserID = item.UserID;
                ur.EventID = item.EventID;
                ur.TimeStamp = item.TimeStamp;
                ur.Status = item.Status;
                ur.EventName = item.Event.EventName;
                ur.EventStartTime = item.Event.StartTime;
                ur.EventEndTime = item.Event.EndTime;
                ur.EventDescription = item.Event.Description;
                userRegistrations.Add(ur);
            }
            return userRegistrations;
        }
    }
}