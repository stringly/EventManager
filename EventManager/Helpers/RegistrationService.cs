using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventManager.ViewModels;

namespace EventManager.Helpers
{
    //TODO:How do I recognize/prevent conflicts in registrations? May in the registrations service?
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
            r.Status = RegistrationStatus.Pending;
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

        public List<RegistrationsForUserViewModel> GetCurrentRegistrationsByLDAP(string name)
        {
            List<RegistrationsForUserViewModel> viewList = new List<RegistrationsForUserViewModel>();
            int UserId = new UserService().GetUserIDFromLDAP(name);
            List<CURRENT_REGISTRATIONS_BY_USERID_Result> resultList = new List<CURRENT_REGISTRATIONS_BY_USERID_Result>();
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    resultList = _dc.CURRENT_REGISTRATIONS_BY_USERID(UserId).ToList();
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to get current registrations for user: " + UserId);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            foreach(CURRENT_REGISTRATIONS_BY_USERID_Result r in resultList)
            {
                RegistrationsForUserViewModel rvm = new RegistrationsForUserViewModel();
                rvm.Description = r.Description;
                rvm.EndTime = r.EndTime;
                rvm.EventID = r.EventID;
                rvm.EventName = r.EventName;
                rvm.EventOwner = r.EventOwner;
                rvm.EventOwnerEmail = r.EventOwnerEmail;
                rvm.RegistrationID = r.RegistrationID;
                rvm.StartTime = r.StartTime;
                rvm.Status = (RegistrationStatus)r.Status;
                rvm.TimeStamp = r.TimeStamp;
                rvm.TotalHours = Convert.ToDouble(r.TotalHours);
                viewList.Add(rvm);
            }

            return viewList;
        }

        public bool UpdateRegistrationStatus(int registrationID, RegistrationStatus newStatus)
        {
            bool result = false;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    Registration r = _dc.Registrations.Where(x => x.RegistrationID == registrationID).FirstOrDefault();
                    r.Status = newStatus;
                    _dc.SaveChanges();
                    result = true;
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to update registration number: " + registrationID);
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