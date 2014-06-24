using ChannelManager.EF;
using System;
using System.Linq;

namespace ChannelManager
{
    public class RoleProvider : System.Web.Security.RoleProvider 
    {
        public enum Roles { admin };

        public override string ApplicationName
        {
            get { return "ChannelManager"; }
            set { }
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
            if (!string.IsNullOrEmpty(username) && IsUserInRole(username, Roles.admin.ToString()))
            {
                return new string[] { Roles.admin.ToString() };
            }
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == Roles.admin.ToString())
            {
                using (var dc = new RepositoryContext())
                {
                    return dc.Users.Any(u => u.Login == username && u.Roles.Any(r => r.Name == roleName));
                }
            }
            return false;
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