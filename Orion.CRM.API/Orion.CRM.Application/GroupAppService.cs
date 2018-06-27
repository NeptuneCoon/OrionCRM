using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class GroupAppService
    {
        private GroupDataAdapter adapter = new GroupDataAdapter();
        public int InsertGroup(Entity.Group group)
        {
            return adapter.InsertGroup(group);
        }

        public bool UpdateGroup(Entity.Group group)
        {
            return adapter.UpdateGroup(group);
        }

        public bool DeleteGroup(int id)
        {
            return adapter.DeleteGroup(id);
        }

        public Entity.Group GetGroupById(int id)
        {
            return adapter.GetGroupById(id);
        }

        public IEnumerable<Entity.Group> GetGroupsByProjectId(int orgId)
        {
            return adapter.GetGroupsByProjectId(orgId);
        }

        public IEnumerable<Entity.Group> GetGroupsByOrgId(int orgId)
        {
            return adapter.GetGroupsByOrgId(orgId);
        }

        // 根据业务组下成员数
        public int GetGroupMemberCountByGroupId(int groupId)
        {
            return adapter.GetGroupMemberCountByGroupId(groupId);
        }
    }
}
