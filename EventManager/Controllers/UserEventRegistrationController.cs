﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.ViewModels;
using EventManager.Models;
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
            //string nameWithoutDomain = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1);
            //user = _dc.Users.Where(u => u.LDAPName == nameWithoutDomain).FirstOrDefault();
            int id = Convert.ToInt32(System.Web.HttpContext.Current.Cache["userID"].ToString());
            user = _dc.Users.Where(u => u.UserId == id).FirstOrDefault();
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
            //var registrationList = _dc.Registrations.Where(e => e.UserID == user.UserId
            //                            && e.Event.StartTime >= DateTime.Now
            //                            && (e.Status != RegistrationStats.Deleted
            //                            && e.Status != RegistrationStats.Declined
            //                            && e.Status != RegistrationStats.NoShow)).ToList();

            //I changed the below because I wanted the view to be able to show the user all of their registrations
            var registrationList = _dc.Registrations.Where(e => e.UserID == user.UserId).OrderByDescending(e => e.Event.StartTime).ToList();

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


        //TODO:Add validation
        [HttpPost]
        public JsonResult RegisterForEvent(Event e)
        {
            var status = false;
            try
            {
                
                DBInteractions db = new DBInteractions();
                int id = Convert.ToInt32(System.Web.HttpContext.Current.Cache["userID"].ToString());
                status = db.Register(e.EventID, id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CancelRegistration(Registration r)
        {
            var status = false;
            try
            {
                DBInteractions db = new DBInteractions();
                status = db.EditRegistration(r.RegistrationID, RegistrationStats.Deleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}