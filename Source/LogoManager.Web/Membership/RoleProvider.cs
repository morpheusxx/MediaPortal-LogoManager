using ChannelManager.EF;
using System;
using System.Linq;

namespace ChannelManager
{
    public class RoleProvider : System.Web.Security.RoleProvider 
    {
        public enum Roles { Administrator, Maintainer };

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
            if (!string.IsNullOrEmpty(username))
            {
                using (var dc = new RepositoryContext("LogoDB"))
                {
                    return dc.Users.Include("Roles").FirstOrDefault(u => u.Login == username)?.Roles.Select(r => r.Name).ToArray() ?? new string[0];
                }
            }
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(roleName))
            {
                using (var dc = new RepositoryContext("LogoDB"))
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