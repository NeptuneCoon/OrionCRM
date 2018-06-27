using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class GroupDataAdapter : DataAdapter
    {
        public int InsertGroup(Entity.Group group)
        {
            if (group == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@OrgId", group.OrgId),
                new SqlParameter("@GroupName", group.GroupName),
                new SqlParameter("@ProjectId", group.ProjectId),
                new SqlParameter("@CreateTime", group.CreateTime),
                new SqlParameter("@UpdateTime", group.UpdateTime),
                new SqlParameter("@ManagerId", CheckNull(group.ManagerId))
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("GroupDomain", "InsertGroup", paramArr);
            return identityId;
        }

        public bool UpdateGroup(Entity.Group group)
        {
            if (group == null || group.Id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", group.Id),
                new SqlParameter("@OrgId", group.OrgId),
                new SqlParameter("@GroupName", group.GroupName),
                new SqlParameter("@ProjectId", group.ProjectId),
                new SqlParameter("@UpdateTime", group.UpdateTime),
                new SqlParameter("@ManagerId",  CheckNull(group.ManagerId))
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("GroupDomain", "UpdateGroup", paramArr);
            return count > 0;
        }

        public bool DeleteGroup(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@GroupId", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("GroupDomain", "DeleteGroup", param);

            return count > 0;
        }

        public Entity.Group GetGroupById(int id)
        {
            if (id <= 0) return null;

            SqlParameter param = new SqlParameter("@Id", id);
            var entity = SqlMapHelper.GetSqlMapSingleResult<Entity.Group>("GroupDomain", "GetGroupById", param);

            return entity;
        }

        public IEnumerable<Entity.Group> GetGroupsByProjectId(int projectId)
        {
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            var groups = SqlMapHelper.GetSqlMapResult<Entity.Group>("GroupDomain", "GetGroupsByProjectId", param);

            return groups;
        }

        public IEnumerable<Entity.Group> GetGroupsByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            var groups = SqlMapHelper.GetSqlMapResult<Entity.Group>("GroupDomain", "GetGroupsByOrgId", param);

            return groups;
        }

        // 根据业务组下成员数
        public int GetGroupMemberCountByGroupId(int groupId)
        {
            SqlParameter param = new SqlParameter("@GroupId", groupId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("GroupDomain", "GetGroupMemberCountByGroupId", param);
            return count;
        }
    }
}
