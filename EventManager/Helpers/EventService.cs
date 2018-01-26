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
        //TODO: Limit AVAILABLE_EVENTS_BY_USERID SP to exclude MaxStaffed events?
        /// <summary>
        /// This Method returns a list of events for which the user is eligible
        /// It is populated by the AVAILABLE_EVENTS_BY_USERID Stored Proc
        /// This stored proc excludes past events
        /// </summary>
        /// <param name="name">The LDAP name of the current user</param>
        /// <returns>List of AvailableEventViewModel objects</returns>
        public List<AvailableEventsViewModel> GetAvailableEventsByLDAP(string name)
        {
            
            List<AvailableEventsViewModel> viewList = new List<AvailableEventsViewModel>();
            int UserId = new UserService().GetUserIDFromLDAP(name);
            List<AVAILABLE_EVENTS_BY_USERID_Result> resultList =new List<AVAILABLE_EVENTS_BY_USERID_Result>();
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {

                try
                {
                    resultList = _dc.AVAILABLE_EVENTS_BY_USERID(UserId).ToList();
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
            foreach(AVAILABLE_EVENTS_BY_USERID_Result e in resultList)
            {
                AvailableEventsViewModel a = new AvailableEventsViewModel();
                a.EventID = e.EventID;
                a.EventName = e.EventName;
                a.StartTime = e.StartTime;
                a.EndTime = e.EndTime;
                a.TotalHours = Convert.ToDouble(e.TotalHours);
                a.Description = e.Description;
                a.AvailableStaff = Convert.ToInt32(e.AvailableStaff);
                a.EventOwner = e.OwnedBy;
                viewList.Add(a);
            }


            return viewList;
        }

        public List<EventsCalendarViewModel> GetCalendarEvents()
        {
            List<EventsCalendarViewModel> viewList = new List<EventsCalendarViewModel>();
            List<EVENTS_LAST_6_MONTHS_Result> resultList = new List<EVENTS_LAST_6_MONTHS_Result>();

            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    resultList = _dc.EVENTS_LAST_6_MONTHS().ToList(); 
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find calendar view events.");
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            foreach(EVENTS_LAST_6_MONTHS_Result r in resultList)
            {
                EventsCalendarViewModel a = new EventsCalendarViewModel();
                a.Description = r.Description;
                a.DisplayColor = r.DisplayColor;
                a.EndTime = r.EndTime;
                a.StartTime = r.StartTime;
                a.EnteredBy = r.EnteredBy;
                a.EventID = r.EventID;
                a.EventName = r.EventName;
                a.FundCenter = r.FundCenter;
                a.MaxStaff = r.MaxStaff;
                a.MinStaff = r.MinStaff;
                viewList.Add(a);               
            }
            return viewList;
        }
    }
}