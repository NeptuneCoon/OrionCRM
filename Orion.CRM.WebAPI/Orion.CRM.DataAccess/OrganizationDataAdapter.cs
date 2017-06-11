using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Orion.CRM.Core;

namespace Orion.CRM.DataAccess
{
    public class OrganizationDataAdapter
    {
        public Entity.Organization GetOrganizationById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);

            var organization = SqlMapHelper.GetSqlMapSingleResult<Entity.Organization>("OrganizationDomain", "GetOrganizationById", param);
            return organization;
        }

        public IEnumerable<Entity.Organization> GetAllOrganizations()
        {
            var orgs = SqlMapHelper.GetSqlMapResult<Entity.Organization>("OrganizationDomain", "GetAllOrganizations", null);
            return orgs;
        }

        public int InsertOrganization(Entity.Organization org)
        {
            if (org == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@OrgName",org.OrgName),
                new SqlParameter("@OrgCode", org.OrgCode),
                new SqlParameter("@Type", org.Type),
                new SqlParameter("@CreateTime", org.CreateTime),
                new SqlParameter("@DeleteFlag", org.DeleteFlag)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("OrganizationDomain", "InsertOrganization", paramArr);
            return identityId;
        }

        public bool UpdateOrganization(Entity.Organization org)
        {
            if (org == null || org.Id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", org.Id),
                new SqlParameter("@OrgName",org.OrgName),
                new SqlParameter("@OrgCode", org.OrgCode),
                new SqlParameter("@Type", org.Type),
                new SqlParameter("@DeleteFlag", org.DeleteFlag)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("OrganizationDomain", "UpdateOrganization", paramArr);
            return count > 0;
        }
    }
}
