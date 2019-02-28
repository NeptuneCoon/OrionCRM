﻿using Orion.CRM.Core;
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

        public Entity.Customer GetCustomerById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            Entity.Customer customer = SqlMapHelper.GetSqlMapSingleResult<Entity.Customer>("Customer", "GetCustomerById", param);

            return customer;
        }

        public IEnumerable<Entity.Customer> GetCustomersByCondition(Entity.CustomerSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("Customer", "GetCustomersByCondition").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", param.pi.ToString()).Replace("$PageSize", param.ps.ToString());

            string sqlWhere = GetSqlWhere(param);
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);
            IEnumerable<Entity.Customer> customers = SqlMapHelper.GetSqlMapResult<Entity.Customer>(mapDetail);

            return customers;
        }

        public int GetCustomersCountByCondition(Entity.CustomerSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("Customer", "GetCustomersCountByCondition").Clone();

            string sqlWhere = GetSqlWhere(param);
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>(mapDetail);
            return count;
        }

        public int AssignServiceUser(string customerIds, int userId)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("Customer", "AssignServiceUser").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$CustomerIds", customerIds);

            SqlParameter parameter = new SqlParameter("@UserId", userId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail, parameter);
            return count;
        }

        public IEnumerable<Entity.Customer> GetCustomersByZone1(string agentZone1)
        {
            SqlParameter param = new SqlParameter("@AgentZone1", agentZone1);
            IEnumerable<Entity.Customer> customers = SqlMapHelper.GetSqlMapResult<Entity.Customer>("Customer", "GetCustomersByZone1", param);
            return customers;
        }




        // 以下3个方法是[客户服务记录CustomerServiceRecord表]有关操作
        public int InsertServiceRecord(Entity.CustomerServiceRecord record)
        {
            if (record == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@CustomerId", record.CustomerId),
                new SqlParameter("@ServiceContent", record.ServiceContent),
                new SqlParameter("@Images", CheckNull(record.Images)),
                new SqlParameter("@AppendUserId",record.AppendUserId)
            };

            int id = SqlMapHelper.ExecuteSqlMapScalar<int>("CustomerServiceRecord", "InsertServiceRecord", paramArr);
            return id;
        }

        public bool DeleteServiceRecord(int id)
        {
            if (id <= 0) return false;
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("CustomerServiceRecord", "DeleteServiceRecord", param);
            return count > 0;
        }


        public IEnumerable<Entity.CustomerServiceRecord> GetServiceRecordsByCustomerId(int customerId)
        {
            SqlParameter param = new SqlParameter("@CustomerId", customerId);
            var records = SqlMapHelper.GetSqlMapResult<Entity.CustomerServiceRecord>("CustomerServiceRecord", "GetServiceRecordsByCustomerId", param);

            return records;
        }


        public Entity.CustomerServiceRecord CustomerServiceRecord(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            Entity.CustomerServiceRecord record = SqlMapHelper.GetSqlMapSingleResult<Entity.CustomerServiceRecord>("CustomerServiceRecord", "GetServiceRecord", param);

            return record;
        }

        #region 生成SQL查询where子句
        private string GetSqlWhere(Entity.CustomerSearchParams param)
        {
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(param.name))
            {
                sqlWhere += $" and A.RealName like '%{param.name}%'";
            }
            if (!string.IsNullOrEmpty(param.mobile))
            {
                sqlWhere += $" and A.Mobile='{param.mobile}'";
            }
            if (param.pid != null && param.pid > 0)
            {
                sqlWhere += $" and A.ProjectId={param.pid}";
            }
            if (param.bid != null && param.bid > 0)
            {
                sqlWhere += $" and A.BrandId={param.bid}";
            }
            if (param.level != null && param.level > 0)
            {
                sqlWhere += $" and A.AgentLevel={param.level}";
            }
            if (!string.IsNullOrEmpty(param.z1))
            {
                sqlWhere += $" and A.AgentZone1='{param.z1}'";
            }
            if (!string.IsNullOrEmpty(param.z2))
            {
                sqlWhere += $" and A.AgentZone2='{param.z2}'";
            }
            if (!string.IsNullOrEmpty(param.z3))
            {
                sqlWhere += $" and A.AgentZone3='{param.z3}'";
            }
            if (param.suid != null && param.suid > 0)
            {
                sqlWhere += $" and A.ServiceUserId={param.suid}";
            }

            return sqlWhere;
        } 
        #endregion
    }
}
