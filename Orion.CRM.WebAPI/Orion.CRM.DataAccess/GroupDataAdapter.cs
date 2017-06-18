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

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("Group", "InsertGroup", paramArr);
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

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Group", "UpdateGroup", paramArr);
            return count > 0;
        }

        public bool DeleteGroup(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Group", "DeleteGroup", param);

            return count > 0;
        }

        public Entity.Group GetGroupById(int id)
        {
            if (id <= 0) return null;

            SqlParameter param = new SqlParameter("@Id", id);
            var entity = SqlMapHelper.GetSqlMapSingleResult<Entity.Group>("Group", "GetGroupById", param);

            return entity;
        }

        public IEnumerable<Entity.Group> GetGroupsByProjectId(int projectId)
        {
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            var groups = SqlMapHelper.GetSqlMapResult<Entity.Group>("Group", "GetGroupsByProjectId", param);

            return groups;
        }

        public IEnumerable<Entity.Group> GetGroupsByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            var groups = SqlMapHelper.GetSqlMapResult<Entity.Group>("Group", "GetGroupsByOrgId", param);

            return groups;
        }
    }
}
