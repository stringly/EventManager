using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManager.ViewModels;
using System.Data.SqlClient;

namespace EventManager.Helpers
{
    /// <summary>
    /// This class provides interface to the database for operations on the Events entity
    /// </summary>
    public class EventService
    {
        //TODO: Rethink userID in cache?
        public List<AvailableEventsViewModel> GetAvailableEventsByLDAP(string name)
        {
            List<Event> eventList = null;
            List<AvailableEventsViewModel> viewList = null;
            int UserId = new UserService().GetUserIDFromLDAP(name);
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {

                try
                {
                    //TODO: Stored Proc, with User ID as a parameter and date filter in SQL? Also needs to not return full events.
                   // eventList = _dc.Events.Where(e => !_dc.Registrations.Any(r => r.UserID == UserId && r.EventID == e.EventID)
                   //                && e.StartTime >= DateTime.Now)
                   //.OrderBy(e => e.StartTime)
                   //.ToList();
                    eventList = _dc.Events.Where(e => !e.Registrations.Any(r => r.UserID == UserId) && e.Registrations.Count(r => r.Status == RegistrationStats.Confirmed) < e.MaxStaff)
                        .OrderBy(e => e.StartTime)
                        .ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find available events for user: " + UserId);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            foreach(Event e in eventList)
            {
                AvailableEventsViewModel a = new AvailableEventsViewModel();
                a.EventName = e.EventName;
                a.StartTime = e.StartTime;
                a.EndTime = e.EndTime;
                a.TotalHours = Math.Round(Convert.ToDecimal((e.EndTime - e.StartTime).TotalHours), 2);
                a.Description = e.Description;
                a.AvailableStaff = e.MaxStaff - e.Registrations.Count(r => r.Status == RegistrationStats.Confirmed);
                a.EventOwner = e.User.Email;
                viewList.Add(a);                
            }
            return viewList;
        }


    }
}