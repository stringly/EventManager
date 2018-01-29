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
        /// This Method returns a list of events for which the user is eligible to register
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

        /// <summary>
        /// This method returns a list of EventsCalendarViewModel objects to pass async to the Calendar view
        /// Used by Events/CalendarView
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method thaqt returns a list of All the current (not past) events in the database
        /// Uses the ALL_EVENTS_VIEW SP to generate the list
        /// </summary>
        /// <returns>List of EventsViewModel objects</returns>
        public List<EventsViewModel> GetAllEvents()
        {
            List<EventsViewModel> viewList = new List<EventsViewModel>();
            List<ALL_EVENTS_VIEW_Result> resultList = new List<ALL_EVENTS_VIEW_Result>();

            using(EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    resultList = _dc.ALL_EVENTS_VIEW().ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find AllEvents.");
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            foreach(ALL_EVENTS_VIEW_Result r in resultList)
            {
                EventsViewModel e = new EventsViewModel();
                e.AvailableStaff = Convert.ToInt32(r.AvailableStaff);
                e.Description = r.Description;
                e.EndTime = r.EndTime;
                e.EventID = r.EventID;
                e.EventName = r.EventName;
                e.EventOwner = r.EventOwner;
                e.EventOwnerEmail = r.EventOwnerEmail;
                e.EventOwnerID = r.EventOwnerID;
                e.StartTime = r.StartTime;
                e.TotalCurrentRegistrations = Convert.ToInt32(r.TotalCurrentRegistrations);
                e.TotalHours = Convert.ToDouble(r.TotalHours);
                viewList.Add(e);

            }
            return viewList;
        }

        public List<MyEventsViewModel> GetMyEvents(int userID)
        {
            List<MyEventsViewModel> viewList = new List<MyEventsViewModel>();
            List<MY_EVENTS_VIEW_Result> resultList = new List<MY_EVENTS_VIEW_Result>();

            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    resultList = _dc.MY_EVENTS_VIEW(userID).ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find Events for user: " + userID);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            foreach (MY_EVENTS_VIEW_Result r in resultList)
            {
                MyEventsViewModel e = new MyEventsViewModel();
                e.DisplayColor = r.DisplayColor;
                e.AvailableStaff = Convert.ToInt32(r.AvailableStaff);
                e.Description = r.Description;
                e.EnteredBy = r.EnteredBy;
                e.FundCenter = r.FundCenter;
                e.MaxStaff = r.MaxStaff;
                e.MinStaff = r.MinStaff;
                e.EndTime = r.EndTime;
                e.EventID = r.EventID;
                e.EventName = r.EventName;
                e.StartTime = r.StartTime;
                e.TotalCurrentRegistrations = Convert.ToInt32(r.TotalCurrentRegistrations);
                e.TotalHours = Convert.ToInt32(r.TotalHours);
                viewList.Add(e);
            }
            return viewList;
        }

        public ManageEventViewModel GetEventToManage(int eventID)
        {
            ManageEventViewModel vm = new ManageEventViewModel();
            List<Registration> resultList = new List<Registration>();
            List<RegistrationDetail> detailsList = new List<RegistrationDetail>();
            using(EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    vm.Event = _dc.Events.Where(x => x.EventID == eventID).FirstOrDefault();
                    resultList = _dc.Registrations.Where(r => r.EventID == eventID).ToList();
                    foreach (Registration r in resultList)
                    {
                        RegistrationDetail rd = new RegistrationDetail();
                        rd.Registration = r;
                        UserDetails ud = new UserDetails();
                        ud.User = _dc.Users.Where(u => u.UserId == r.UserID).FirstOrDefault();
                        ud.UserRank = _dc.LTRanks.Where(rk => rk.RankId == rd.UserDetails.User.Rank).FirstOrDefault();
                        CONFIRMED_HOUR_TOTALS_BY_USER_Result h = _dc.CONFIRMED_HOUR_TOTALS_BY_USER(r.UserID).FirstOrDefault();
                        ud.TotalRegistrations = Convert.ToInt32(h.TotalRegistrations);
                        ud.TotalHoursLast30Days = Convert.ToInt32(h.TotalHoursLast30Days);
                        ud.TotalHoursLast60Days = Convert.ToInt32(h.TotalHoursLast60Days);
                        ud.TotalHoursLast90Days = Convert.ToInt32(h.TotalHoursLast90Days);
                        rd.UserDetails = ud;
                        detailsList.Add(rd);
                    }
                    vm.CurrentRegistrations = detailsList;
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to create a Management viewmodel for event: " + eventID);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return vm;
        }
    }
}