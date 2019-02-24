using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class CustomerDataAdapter : DataAdapter
    {
        public int InsertCustomer(Entity.Customer customer)
        {
            if (customer == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@RealName", customer.RealName),
                new SqlParameter("@Sex", CheckNull(customer.Sex)),
                new SqlParameter("@Mobile", CheckNull(customer.Mobile)),
                new SqlParameter("@IdentityNo", CheckNull(customer.IdentityNo)),
                new SqlParameter("@BrandId", customer.BrandId),
                new SqlParameter("@AgentLevel", customer.AgentLevel),
                new SqlParameter("@AgentZone1", CheckNull(customer.AgentZone1)),
                new SqlParameter("@AgentZone2", CheckNull(customer.AgentZone2)),
                new SqlParameter("@AgentZone3", CheckNull(customer.AgentZone3)),
                new SqlParameter("@ProjectId", customer.ProjectId),
                new SqlParameter("@ServiceUserId", CheckNull(customer.ServiceUserId))
            };

            int id = SqlMapHelper.ExecuteSqlMapScalar<int>("Customer", "InsertCustomer", paramArr);
            return id;
        }

        public bool UpdateCustomer(Entity.Customer customer)
        {
            if (customer == null || customer.Id <= 0) return false;


            SqlParameter[] paramArr = {
                new SqlParameter("@Id", customer.Id),
                new SqlParameter("@RealName", customer.RealName),
                new SqlParameter("@Sex", CheckNull(customer.Sex)),
                new SqlParameter("@Mobile", CheckNull(customer.Mobile)),
                new SqlParameter("@IdentityNo", CheckNull(customer.IdentityNo)),
                new SqlParameter("@BrandId", customer.BrandId),
                new SqlParameter("@AgentLevel", customer.AgentLevel),
                new SqlParameter("@AgentZone1", CheckNull(customer.AgentZone1)),
                new SqlParameter("@AgentZone2", CheckNull(customer.AgentZone2)),
                new SqlParameter("@AgentZone3", CheckNull(customer.AgentZone3)),
                new SqlParameter("@ProjectId", customer.ProjectId),
                new SqlParameter("@ServiceUserId", CheckNull(customer.ServiceUserId)),
                new SqlParameter("@UpdateTime", DateTime.Now)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Customer", "UpdateCustomer", paramArr);
            return count > 0;
        }
    }
}
