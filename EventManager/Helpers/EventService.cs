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
        public IEnumerable<AvailableEventsViewModel> GetAvailableEventsByLDAP(string name)
        {
            IEnumerable<Event> eventList = null;
            IEnumerable<AvailableEventsViewModel> viewList = null;
            int UserId = new UserService().GetUserIDFromLDAP(name);
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {

                try
                {
                    //TODO: Stored Proc, with User ID as a parameter and date filter in SQL? Also needs to not return full events.
                    eventList = _dc.Events.Where(e => !_dc.Registrations.Any(r => r.UserID == UserId && r.EventID == e.EventID)
                                   && e.StartTime >= DateTime.Now)
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
                a.TotalHours = DateTimeOffset()
            }


            return viewList;
        }


    }
}