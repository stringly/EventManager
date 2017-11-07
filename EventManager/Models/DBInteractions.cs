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
                    _db.SaveChanges();
                    result = true;
                }
            }
            return result;
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
                            events.Add(n);
                        }
                        _db.Events.AddRange(events);
                        _db.SaveChanges();
                        result = true;
                        break;
                    case 3: //Week Repeater
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
    }
}