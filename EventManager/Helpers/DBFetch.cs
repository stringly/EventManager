using EventManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EventManager.Helpers
{
    public class DBFetch
    {
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
                        uvm.ContactNumber = u.ContactNumber;
                        uvm.LDAPName = u.LDAPName;
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
                    u.ContactNumber = uvm.ContactNumber;
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
                        u.ContactNumber = uvm.ContactNumber;
                        u.LDAPName = uvm.LDAPName;
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
    }
}