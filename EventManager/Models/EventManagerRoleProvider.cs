using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Security;

namespace EventManager.Models
{
    public class EventManagerRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                string nameWithoutDomain = username.Substring(username.LastIndexOf(@"\") + 1);
                User user = _dc.Users.FirstOrDefault(u => u.LDAPName.Equals(nameWithoutDomain, StringComparison.CurrentCultureIgnoreCase));

                if (user != null)
                {
                    var roles = from ur in user.Roles
                                from r in _dc.Roles
                                where ur.RoleId == r.RoleId
                                select r.Name;
                    return roles.ToArray();
                }
                    
                else
                    return new string[] { };
                
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (EVENTS_MGR_TESTING_Entities _dc = new EVENTS_MGR_TESTING_Entities())
            {
                User user = _dc.Users.FirstOrDefault(u => u.LDAPName.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                var roles = from ur in user.Roles
                            from r in _dc.Roles
                            where ur.RoleId == r.RoleId
                            select r.Name;
                if (user != null)
                    return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}