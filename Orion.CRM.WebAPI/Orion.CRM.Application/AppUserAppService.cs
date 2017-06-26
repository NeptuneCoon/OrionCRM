using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class AppUserAppService
    {
        private AppUserDataAdapter adapter = new AppUserDataAdapter();

        public IEnumerable<Entity.AppUser> GetUsers(int pageIndex, int pageSize)
        {
            return adapter.GetUsers(pageIndex, pageSize);
        }

        public IEnumerable<Entity.AppUser> GetUsersByOrgId(int pageIndex, int pageSize, int orgId)
        {
            return adapter.GetUsersByOrgId(pageIndex, pageSize, orgId);
        }

        public IEnumerable<Entity.AppUser> GetAllUsersByGroupId(int groupId)
        {
            return adapter.GetAllUsersByGroupId(groupId);
        }

        public Entity.AppUser GetUserById(int id)
        {
            return adapter.GetUserById(id);
        }

        public Entity.AppUser GetUserByUserName(string userName)
        {
            return adapter.GetUserByUserName(userName);
        }

        public int InsertUser(Entity.AppUser user)
        {
            return adapter.InsertUser(user);
        }

        public bool UpdateUser(Entity.AppUser user)
        {
            return adapter.UpdateUser(user);
        }

        public bool UpdatePassword(string userId, string password)
        {
            return adapter.UpdatePassword(userId, password);
        }

        public int GetUserCount()
        {
            return adapter.GetUserCount();
        }

        public int GetUserCountByOrgId(int orgId)
        {
            return adapter.GetUserCountByOrgId(orgId);
        }

        public int InsertUserRole(Entity.UserRole userRole)
        {
            return adapter.InsertUserRole(userRole);
        }

        public bool UpdateUserRole(Entity.UserRole userRole)
        {
            return adapter.UpdateUserRole(userRole);
        }

        public int InsertUserProject(Entity.UserProject userProject)
        {
            return adapter.InsertUserProject(userProject);
        }

        public bool UpdateUserProject(Entity.UserProject userProject)
        {
            return adapter.UpdateUserProject(userProject);
        }

        public int InsertUserGroup(Entity.UserGroup userGroup)
        {
            return adapter.InsertUserGroup(userGroup);
        }

        public bool UpdateUserGroup(Entity.UserGroup userGroup)
        {
            return adapter.UpdateUserGroup(userGroup);
        }
    }
}
