using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EventManager.Helpers
{
    public class RegistrationService
    {
        /// <summary>
        /// Method to create and write a new Registration to the Database
        /// </summary>
        /// <param name="userID">The Database UserId(int)</param>
        /// <param name="eventID">The Database EventId(int)</param>
        /// <returns><c>true</c> if the registration was added successfully; otherwise, <c>false</c></returns>
        public bool CreateRegistration(int userID, int eventID)
        {
            bool result = false;
            Registration r = new Registration();
            r.EventID = eventID;
            r.UserID = userID;
            r.TimeStamp = DateTime.Now;
            r.Status = RegistrationStats.Pending;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    _dc.Registrations.Add(r);
                    _dc.SaveChanges();
                    result = true;
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to create registration with UserId: " + userID + " and EventId: " + eventID);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return result;
        }
    }
}