using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class CustomerSignDataAdapter : DataAdapter
    {
        public int InsertSign(Entity.CustomerSign sign)
        {
            if (sign == null) return -1;

            SqlParameter[] parameters = {
                new SqlParameter("@Amount", sign.Amount),
                new SqlParameter("@CustomerName", CheckNull(sign.CustomerName)),
                new SqlParameter("@CustomerPhone", CheckNull(sign.CustomerPhone)),
                new SqlParameter("@ResourceId", CheckNull(sign.ResourceId)),
                new SqlParameter("@SignTime", sign.SignTime),
                new SqlParameter("@SignUserId", sign.SignUserId),
                new SqlParameter("@SignMan", sign.SignMan),
                new SqlParameter("@GroupId", CheckNull(sign.GroupId)),
                new SqlParameter("@GroupName", CheckNull(sign.GroupName)),
                new SqlParameter("@ProjectId", sign.ProjectId),
                new SqlParameter("@OrgId", sign.OrgId),
                new SqlParameter("@CreateTime", sign.CreateTime),
                new SqlParameter("@AppendUserId", sign.AppendUserId),
                new SqlParameter("@AppendMan", sign.AppendMan)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("CustomerSign", "InsertSign", parameters);
            return identityId;
        }

        public int DeleteSign(int resourceId)
        {
            if (resourceId <= 0) return 0;
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("CustomerSign", "DeleteSign", param);
            return count;
        }

        public int Delete(int id)
        {
            if (id <= 0) return 0;
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("CustomerSign", "Delete", param);
            return count;
        }

        public int DeleteSignByProjectId(int projectId)
        {
            if (projectId <= 0) return 0;
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("CustomerSign", "DeleteSignByProjectId", param);
            return count;
        }

        public Entity.CustomerSign GetSignByResourceId(int resourceId)
        {
            if (resourceId <= 0) return null;
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            Entity.CustomerSign entity = SqlMapHelper.GetSqlMapSingleResult<Entity.CustomerSign>("CustomerSign", "GetSignByResourceId", param);
            return entity;
        }

        public IEnumerable<Entity.CustomerSign> GetSignsByTime(int orgId, string beginTime, string endTime)
        {
            if (orgId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@OrgId", orgId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetSignsByTime", paramArr);
            return result;
        }

        public IEnumerable<Entity.CustomerSign> GetGroupMemberSigns(int groupId, string beginTime, string endTime)
        {
            if (groupId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@GroupId", groupId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetGroupMemberSigns", paramArr);
            return result;
        }

        public IEnumerable<Entity.CustomerSign> GetProjectGroupSigns(int projectId, string beginTime, string endTime)
        {
            if (projectId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetProjectGroupSigns", paramArr);
            return result;
        }

        public IEnumerable<Entity.CustomerSign> GetSignsByCondition(Entity.SignSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("CustomerSign", "GetSignsByCondition").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", param.pi.ToString()).Replace("$PageSize", param.ps.ToString());

            string sqlWhere = "";
            if (!string.IsNullOrEmpty(param.name)) {
                sqlWhere += $" and CustomerName like '%{param.name}%'";
            }
            if (!string.IsNullOrEmpty(param.con)) {
                //sqlWhere += $" and (Mobile='{param.con}' or Tel='{param.con}' or Wechat='{param.con}' or QQ='{param.con}')";
                sqlWhere += $" and CustomerPhone='{param.con}')";
            }
            if (param.uid > 0) {
                sqlWhere += $" and SignUserId=" + param.uid;
            }
            if (param.pid > 0) {
                sqlWhere += $" and ProjectId=" + param.pid;
            }
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);


            SqlParameter parameter = new SqlParameter("@OrgId", param.oid);
            IEnumerable<Entity.CustomerSign> users = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>(mapDetail, parameter);

            return users;
        }

        public int GetSignCountByCondition(Entity.SignSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("CustomerSign", "GetSignCountByCondition").Clone();

            string sqlWhere = "";
            if (!string.IsNullOrEmpty(param.name)) {
                sqlWhere += $" and CustomerName like '%{param.name}%'";
            }
            if (!string.IsNullOrEmpty(param.con)) {
                sqlWhere += $" and CustomerPhone='{param.con}')";
            }
            if (param.uid > 0) {
                sqlWhere += $" and SignUserId=" + param.uid;
            }
            if (param.pid > 0) {
                sqlWhere += $" and ProjectId=" + param.pid;
            }
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);


            SqlParameter parameter = new SqlParameter("@OrgId", param.oid);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>(mapDetail, parameter);
            return count;
        }

        /// <summary>
        /// 获取某段时间内各组业绩
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity.GroupSaleRanking> GetGroupSaleRanking(int orgId, int projectId,string beginTime,string endTime)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrgId", orgId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime),
            };

            var  result = SqlMapHelper.GetSqlMapResult<Entity.GroupSaleRanking>("CustomerSign", "GetGroupSaleRanking", parameters);
            return result;
        }
    }
}
