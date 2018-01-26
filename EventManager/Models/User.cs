using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventManager.Helpers;
using System.Data.Entity;

namespace EventManager
{
    /// <summary>
    /// Represents a User in the User Table of the Database and provides the associated interactions.
    /// This is an extenstion of the EDMX partial class by the same name, which contains the properties, collections, 
    /// and default constructor.
    /// </summary>
    public partial class User
    {



        /// <summary>
        /// Pushes the Current User's ID onto the cache
        /// </summary>
        public void PushUserToCache()
        {
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                if (System.Web.HttpContext.Current.Cache["userID"] == null)
                {
                    System.Web.HttpContext.Current.Cache["userID"] = UserId;
                }
            }
        }
        
        /// <summary>
        /// Locates and returns a user object by UserId
        /// </summary>
        /// <param name="id">The user's database ID (PK)</param>
        /// <returns>A populated user object or a null object if the user cannot be found</returns>
        public static User GetUserByID(int id)
        {
            var u = new User();

            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
            
                
                try
                {
                    //TODO: STORED PROC (Find User By ID(PK))
                    u = _dc.Users.Where(x => x.UserId == id).FirstOrDefault();
                    
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find user by ID.");
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return u;
    }

        /// <summary>
        /// Returns a list of events that are not expired and for which the User has not registered
        /// </summary>
        /// <returns>IEnumerable List of EventManager.Events if list is found; otherwise, null</returns>
        public IEnumerable<Event> GetAvailableEventListForUser()
        {
            IEnumerable<Event> eventList = null;
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
            return eventList;
        }

        /// <summary>
        /// Finds all events owner by the User by UserId
        /// </summary>
        /// <returns>Returns an IEnumerable list of EventManager.Events if events are found; otherwise, returns null</returns>
        public IEnumerable<Event> GetUserOwnedEvents()
        {
            IEnumerable<Event> eventList = null;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    //DateTime dateDiff = DateTime.Today.AddMonths(-6); <==This needs to be implemented in SQL
                    eventList = _dc.Events.Where(e => e.User.UserId == UserId).ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to user-owned events for user: " + UserId);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return eventList;
        }

        /// <summary>
        /// Creates a registration for the user
        /// </summary>
        /// <param name="eventID">EventId for the associated Event</param>
        /// <returns><c>true</c> if the Registration is created successfully; otherwise, <c>false</c></returns> 
        public Boolean Register(int eventID)
        {
            //TODO: should this be a User Method?
            bool result = false;

            using (EVENTS_MGR_TESTING_Entities _db = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    //TODO: this might require re-wiring a registration action
                    Registration r = new Registration();
                    r.EventID = eventID;
                    r.UserID = UserId;
                    r.TimeStamp = DateTime.Now;
                    r.Status = RegistrationStatus.Pending;
                    _db.Registrations.Add(r);
                    _db.SaveChanges();
                    result = true;
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to register user: " + UserId + " for Event number: " + eventID);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return result;
        }

        public IEnumerable<Registration> GetRegistrationsForUser()
        {
            IEnumerable<Registration> regList = null;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    //TODO: Stored Proc
                    regList = _dc.Registrations.Where(r => r.UserID == UserId).Include(r => r.Event).OrderBy(r => r.Event.StartTime).ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find registrations for user: " + UserId);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return regList;
        }
    }
}