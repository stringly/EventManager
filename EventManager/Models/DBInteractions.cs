using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManager.Helpers;
using EventManager.Models;
using System.Diagnostics;

namespace EventManager.Models
{
    public class DBInteractions
    {
        //Event Interactions
        public IEnumerable<DateTime> Weekdays(DateTime from, DateTime thru)
        {
            for(DateTime day = from.Date; day.Date <= thru.Date; day= day.AddDays(1))
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    yield return day;
                }
            }
        }
        public Boolean IsSelectedDay(int[] selectedDays, DateTime day)
        {
            //HACK: Day-of-week test method 7-if-block-stack
            if (selectedDays.Contains(1))
            {
                if (day.DayOfWeek == DayOfWeek.Sunday) { return true; }
            }
            if (selectedDays.Contains(2))
            {
                if (day.DayOfWeek == DayOfWeek.Monday) { return true; }
            }
            if (selectedDays.Contains(3))
            {
                if (day.DayOfWeek == DayOfWeek.Tuesday) { return true; }
            }
            if (selectedDays.Contains(4))
            {
                if (day.DayOfWeek == DayOfWeek.Wednesday) { return true; }
            }
            if (selectedDays.Contains(5))
            {
                if (day.DayOfWeek == DayOfWeek.Thursday) { return true; }
            }
            if (selectedDays.Contains(6))
            {
                if (day.DayOfWeek == DayOfWeek.Friday) { return true; }
            }
            if (selectedDays.Contains(7))
            {
            if (day.DayOfWeek == DayOfWeek.Saturday) { return true; }
            }
            return false;
        }
        public Boolean EditExistingEvent(Event e)
        {
            Boolean result = false;
            using(EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    var v = _db.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.EventName = e.EventName;
                        v.StartTime = e.StartTime;
                        v.EndTime = e.EndTime;
                        v.Description = e.Description;
                        v.MaxStaff = e.MaxStaff;
                        v.MinStaff = e.MinStaff;
                        v.FundCenter = e.FundCenter;
                        v.EnteredBy = e.EnteredBy;
                        v.DisplayColor = e.DisplayColor;
                        _db.SaveChanges();
                        result = true;
                    }
                }
                catch(Exception ex)
                {
                    Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
                }
            return result;
            }

        }
        public Boolean AddNewEvent(Event e)
        {
            Boolean result = false;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                _db.Events.Add(e);
                _db.SaveChanges();
                result = true;
            }
            return result;
        }
        public Boolean RepeatEvent(Event e, EventRepeater r)
        {
            Debug.WriteLine("Repeat method Entered");
            Boolean result = false;
            List<Event> events = new List<Event>();
            var shiftlength = (e.EndTime - e.StartTime).TotalMinutes;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                switch (r.repeatType)
                {
                    case 1: //Daily Repeater
                        Debug.WriteLine("Case 1 entered");
                        DateTime repeatedEventNewStartDate = e.StartTime;
                        DateTime repeatedEventNewEndDate = e.EndTime;
                        
                        for (int i = 1; i < r.repeatCount + 1; i++)
                        {
                            Debug.WriteLine("Loop number" + i);
                            //add a day to the Repeater Start Date
                            Event n = new Event();
                            n.EventName = e.EventName;
                            n.StartTime = repeatedEventNewStartDate.AddDays(i);
                            n.EndTime = repeatedEventNewEndDate.AddDays(i);
                            n.Description = e.Description;
                            n.MaxStaff = e.MaxStaff;
                            n.MinStaff = e.MinStaff;
                            n.FundCenter = e.FundCenter;
                            n.EnteredBy = e.EnteredBy;
                            n.DisplayColor = e.DisplayColor;
                            events.Add(n);                        
                        }
                        _db.Events.AddRange(events);
                        _db.SaveChanges();
                        result = true;
                        break;
                    case 2: //WeekDay Repeater
                        Debug.WriteLine("Case 2 Entered");
                        var timediff = e.StartTime.TimeOfDay;
                        
                        foreach (DateTime day in Weekdays(r.startDate, r.endDate))
                        {
                            Event n = new Event();
                            n.EventName = e.EventName;
                            n.StartTime = day.Add(timediff);
                            n.EndTime = n.StartTime.AddMinutes(shiftlength);
                            n.Description = e.Description;
                            n.MaxStaff = e.MaxStaff;
                            n.MinStaff = e.MinStaff;
                            n.FundCenter = e.FundCenter;
                            n.EnteredBy = e.EnteredBy;
                            n.DisplayColor = e.DisplayColor;
                            events.Add(n);
                        }
                        _db.Events.AddRange(events);
                        _db.SaveChanges();
                        result = true;
                        break;
                    case 3: //Week Repeater
                        //TODO: Week repeater behaves oddly with 1 week selected
                        DateTime stopDate = e.StartTime.AddDays(7 * r.repeatCount);
                        for (DateTime i = e.StartTime.AddDays(1); i.Date < stopDate.Date; i = i.AddDays(1))
                        {
                            if (IsSelectedDay(r.dow, i) == true)
                            {
                                Event n = new Event();
                                n.EventName = e.EventName;
                                n.StartTime = i;
                                n.EndTime = i.AddMinutes(shiftlength);
                                n.Description = e.Description;
                                n.MaxStaff = e.MaxStaff;
                                n.MinStaff = e.MinStaff;
                                n.FundCenter = e.FundCenter;
                                n.EnteredBy = e.EnteredBy;
                                n.DisplayColor = e.DisplayColor;
                                events.Add(n);
                            }
                        }
                        _db.Events.AddRange(events);
                        _db.SaveChanges();
                        result = true;
                        break;

                    default:
                        Debug.WriteLine("Default switch case");
                        break;
            
                }
            }
            return result;
        }
        public Boolean DeleteEvent(int eventID)
        {
            var result = false;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                var v = _db.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    _db.Events.Remove(v);
                    _db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
        public IEnumerable<Event> GetEventListForUser()
        {
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                DateTime dateDiff = DateTime.Today.AddMonths(-6);
                return _dc.Events.Where(x => x.StartTime > dateDiff).ToList();
            }

        }
        public IEnumerable<Event> GetEventListAll()
        {
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {                
                return _dc.Events.ToList();
            }
        }

        //User Interactions
        public Boolean AddNewUser(User u)
        {
            bool result = false;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    _db.Users.Add(u);
                    _db.SaveChanges();
                    result = true;
                }
                catch(Exception e)
                {
                    //TODO: Error Logging
                    Debug.Write(e.Message);
                }
            }
            return result;
        }
        public Boolean EditUser(User u)
        {
            bool result = false;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    var v = _db.Users.Where(a => a.UserId == u.UserId).FirstOrDefault();
                    if (v != null)
                    {
                            v.UserId = u.UserId;
                            v.Rank = u.Rank;
                            v.FirstName = u.FirstName;
                            v.LastName = u.LastName;
                            v.IDNumber = u.IDNumber;
                            v.PayRollID = u.PayRollID;
                            v.Email = u.Email;
                            v.ContactNumber = u.ContactNumber;
                            _db.SaveChanges();
                            result = true;
                    }
                }
                catch (Exception e)
                {
                    //TODO: Add Error Logging
                    Debug.WriteLine(e.Message);
                }
            }
            return result;
        }
        public Boolean Register(int eventID, int userID)
        {
            bool result = false;

            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    Registration r = new Registration();
                    r.EventID = eventID;
                    r.UserID = userID;
                    r.TimeStamp = DateTime.Now;
                    r.Status = RegistrationStats.Pending;
                    _db.Registrations.Add(r);
                    _db.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    //TODO: Add Error Logging
                    Debug.WriteLine(e.Message);
                }
            }
            return result;
        }
        public void PushUserToCache()
        {
            string nameWithoutDomain = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.LastIndexOf(@"\") + 1);
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                User user = new EventManager.User();
                user = _dc.Users.Where(u => u.LDAPName == nameWithoutDomain).FirstOrDefault();
                if (user != null)
                {
                    if (System.Web.HttpContext.Current.Cache["userID"] == null)
                    {
                        System.Web.HttpContext.Current.Cache["userID"] = user.UserId;
                    }
                }
            }
        }
        //Registration Interactions
        public Boolean EditRegistration(int registrationID, RegistrationStats status)
        {
            bool result = false;
            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    Registration r = _db.Registrations.Where(a => a.RegistrationID == registrationID).FirstOrDefault();
                    if (r != null)
                    {
                        r.Status = status;
                        _db.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
                }
            }
            return result;
        }
        public Boolean UpdateRegistrationsByList(List<RegistrationUpdateListItem> list)
        {
            var result = false;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    foreach (RegistrationUpdateListItem u in list)
                    {
                        Registration r = _dc.Registrations.Where(a => a.RegistrationID == u.id).FirstOrDefault();
                        if (r != null)
                        {
                            r.Status = u.status;
                            if (u.notify)
                            {
                                r.NotifyRegistrationStatusChange();
                            }
                            
                        }
                        else
                        {
                            throw new NullReferenceException("Null reference to registration object");
                        }
                    }
                    _dc.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
                }
            }



            return result;
        }


    }
}