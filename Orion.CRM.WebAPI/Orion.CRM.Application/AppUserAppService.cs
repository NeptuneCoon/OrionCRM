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

        public int GetUserCount()
        {
            return adapter.GetUserCount();
        }
    }
}
