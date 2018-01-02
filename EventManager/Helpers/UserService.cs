using EventManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EventManager.Helpers
{
    /// <summary>
    /// This class provides interface to the database for operations on the User entity
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Returns a UserInfoViewModel from the provided User LDAP or returns
        /// a "new" User UserInfoViewModel if the LDAP name isn't found in the DB
        /// </summary>
        /// <param name="name">The LDAP name of the user.</param>
        /// <returns>Populated UserInfoViewModel or "Empty" UserInfoViewModel</returns>
        public UserInfoViewModel GetUserInfoViewModelByLDAP(string name)
        {
            UserInfoViewModel uvm = new UserInfoViewModel();
            try
            {
                using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
                {
                    User u = _dc.Users.Where(x => x.LDAPName == name).FirstOrDefault();
                    if (u != null) //User is known and found in DB
                    {
                        uvm.UserId = u.UserId;
                        uvm.Rank = u.Rank;
                        uvm.FirstName = u.FirstName;
                        uvm.LastName = u.LastName;
                        uvm.IDNumber = u.IDNumber;
                        uvm.Email = u.Email;
                        uvm.PayrollID = u.PayRollID;
                        uvm.LDAPName = u.LDAPName;
                        uvm.ContactNumber = PhoneNumberConverter(u.ContactNumber, 0);
                        
                    }
                    else //User is not in DB
                    {
                        uvm.UserId = 0;
                        uvm.Rank = 1;
                        uvm.FirstName = "";
                        uvm.LastName = "";
                        uvm.IDNumber = 0000;
                        uvm.Email = name + "@co.pg.md.us";
                        uvm.PayrollID = "";
                        uvm.ContactNumber = "";
                        uvm.LDAPName = name;
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to find user: " + name);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
            }
            return uvm;
        }

        /// <summary>
        /// Creates a new Entity User and writes to the DB from the 
        /// UserInfoViewModel provided as a parameter
        /// </summary>
        /// <param name="uvm">A Populated UserInfoViewModel object</param>
        /// <returns><c>true</c> if the user was added successfully; otherwise, <c>false</c></returns>
        public bool CreateUserFromViewModel(UserInfoViewModel uvm)
        {
            bool result = false;

            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    User u = new EventManager.User();
                    u.Rank = uvm.Rank;
                    u.FirstName = uvm.FirstName;
                    u.LastName = uvm.LastName;
                    u.IDNumber = uvm.IDNumber;
                    u.PayRollID = uvm.PayrollID;
                    u.Email = uvm.Email;
                    u.ContactNumber = PhoneNumberConverter(uvm.ContactNumber, 1);
                    u.RegisteredDate = DateTime.Now;
                    u.LDAPName = uvm.LDAPName;
                    Role r = _dc.Roles.Where(x => x.Name == "User").FirstOrDefault();
                    u.Roles.Add(r);
                    _dc.Users.Add(u);
                    _dc.SaveChanges();
                    result = true;

                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to add new user.");
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Updates a DB user from the provided viewmodel
        /// </summary>
        /// <param name="uvm">A populated UserInfoViewModel object</param>
        /// <returns><c>true</c> if the user is updated successfully: otherwise <c>false</c></returns>
        public bool UpdateUserFromViewModel(UserInfoViewModel uvm)
        {
            bool result = false;
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                try
                {
                    User u = _dc.Users.Where(x => x.UserId == uvm.UserId).FirstOrDefault();
                    if (u != null)
                    {
                        u.Rank = uvm.Rank;
                        u.FirstName = uvm.FirstName;
                        u.LastName = uvm.LastName;
                        u.IDNumber = uvm.IDNumber;
                        u.PayRollID = uvm.PayrollID;
                        u.Email = uvm.Email;
                        u.ContactNumber = PhoneNumberConverter(uvm.ContactNumber, 1);
                        _dc.SaveChanges();
                        result = true;
                    }
                }
                catch (SqlException ex)
                {
                    ErrorLog.LogError(ex, "SQL error number: " + ex.Number + " encountered when trying to update user: " + uvm.UserId);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Adds/Removes Special Characters from a phone number
        /// </summary>
        /// <param name="number">The phone number from which to remove/add characters</param>
        /// <param name="ToView">Specify 1 to remove characters or 0/nothing to add characters</param>
        /// <returns>a string with a formatted phone number</returns>
        public string PhoneNumberConverter(string number, int ToView = 0)
        {
            switch (ToView)
            {
                case 1: //Sterilize to save

                    return new string(number.Where(c => char.IsDigit(c)).ToArray());

                default: //Convert to view
                    string n;
                    n = Convert.ToInt64(number).ToString("(###) ###-####");
                    return n;
            }
        }
    }
}