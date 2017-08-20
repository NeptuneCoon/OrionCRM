using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class ProjectDataAdapter : DataAdapter
    {
        public Entity.Project GetProjectById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            var project = SqlMapHelper.GetSqlMapSingleResult<Entity.Project>("ProjectDomain", "GetProjectById", param);

            return project;
        }

        public IEnumerable<Entity.Project> GetProjectsByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            var projects = SqlMapHelper.GetSqlMapResult<Entity.Project>("ProjectDomain", "GetProjectsByOrgId", param);

            return projects;
        }

        public int InsertProject(Entity.Project project)
        {
            if (project == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@OrgId",project.OrgId),
                new SqlParameter("@ProjectName", project.ProjectName),
                new SqlParameter("@CreateTime", project.CreateTime),
                new SqlParameter("@UpdateTime", project.UpdateTime)
                //new SqlParameter("@CreateUserId", project.CreateUserId)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ProjectDomain", "InsertProject", paramArr);
            return identityId;
        }

        public bool UpdateProject(Entity.Project project)
        {
            if (project == null || project.Id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", project.Id),
                new SqlParameter("@OrgId",project.OrgId),
                new SqlParameter("@ProjectName", project.ProjectName),
                new SqlParameter("@UpdateTime", project.UpdateTime)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ProjectDomain", "UpdateProject", paramArr);
            return count > 0;
        }

        public int DeleteProject(int id)
        {
            if (id <= 0) return 0;
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ProjectDomain", "DeleteProject", param);
            return count;
        }
    }
}
