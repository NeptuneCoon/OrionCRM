﻿using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class AppUserDataAdapter : DataAdapter
    {
        public IEnumerable<Entity.AppUser> GetUsers(int pageIndex, int pageSize)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("AppUserDomain", "GetUsers").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());

            IEnumerable<Entity.AppUser> users = SqlMapHelper.GetSqlMapResult<Entity.AppUser>(mapDetail);
            return users;
        }

        public IEnumerable<Entity.AppUser> GetUsersByOrgId(int pageIndex, int pageSize, int orgId)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("AppUserDomain", "GetUsersByOrgId").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());

            SqlParameter param = new SqlParameter("@OrgId", orgId);
            IEnumerable<Entity.AppUser> users = SqlMapHelper.GetSqlMapResult<Entity.AppUser>(mapDetail, param);
            return users;
        }

        public Entity.AppUser GetUserById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            Entity.AppUser appUser = SqlMapHelper.GetSqlMapSingleResult<Entity.AppUser>("AppUserDomain", "GetUserById", param);

            return appUser;
        }

        public Entity.AppUser GetUserByUserName(string userName)
        {
            SqlParameter param = new SqlParameter("@UserName", userName);
            Entity.AppUser appUser = SqlMapHelper.GetSqlMapSingleResult<Entity.AppUser>("AppUserDomain", "GetUserByUserName", param);

            return appUser;
        }

        public int InsertUser(Entity.AppUser user)
        {
            if (user == null) return -1;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@OrgId", user.OrgId));
            parameters.Add(new SqlParameter("@UserName", user.UserName));
            parameters.Add(new SqlParameter("@Password", user.Password));
            parameters.Add(new SqlParameter("@RealName", user.RealName));
            parameters.Add(new SqlParameter("@CreateTime", user.CreateTime));
            parameters.Add(new SqlParameter("@UpdateTime", user.UpdateTime));
            parameters.Add(new SqlParameter("@Enable", user.Enable));
            if (user.Mobile != null) {
                parameters.Add(new SqlParameter("@Mobile", user.Mobile));
            }
            else {
                parameters.Add(new SqlParameter("@Mobile", DBNull.Value));
            }

            if (user.Email != null) {
                parameters.Add(new SqlParameter("@Email", user.Email));
            }
            else {
                parameters.Add(new SqlParameter("@Email", DBNull.Value));
            }

            if (user.Wechat != null) {
                parameters.Add(new SqlParameter("@Wechat", user.Wechat));
            }
            else {
                parameters.Add(new SqlParameter("@Wechat", DBNull.Value));
            }

            SqlParameter[] paramArr = parameters.ToArray();

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "InsertUser", paramArr);
            return identityId;
        }

        public bool UpdateUser(Entity.AppUser user)
        {
            if (user == null || user.Id <= 0) return false;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", user.Id));
            parameters.Add(new SqlParameter("@OrgId", user.OrgId));
            parameters.Add(new SqlParameter("@UserName", user.UserName));
            parameters.Add(new SqlParameter("@Password", user.Password));
            parameters.Add(new SqlParameter("@RealName", user.RealName));
            parameters.Add(new SqlParameter("@UpdateTime", user.UpdateTime));
            parameters.Add(new SqlParameter("@Enable", user.Enable));
            if (user.Mobile != null) {
                parameters.Add(new SqlParameter("@Mobile", user.Mobile));
            }
            else {
                parameters.Add(new SqlParameter("@Mobile", DBNull.Value));
            }

            if (user.Email != null) {
                parameters.Add(new SqlParameter("@Email", user.Email));
            }
            else {
                parameters.Add(new SqlParameter("@Email", DBNull.Value));
            }

            if (user.Wechat != null) {
                parameters.Add(new SqlParameter("@Wechat", user.Wechat));
            }
            else {
                parameters.Add(new SqlParameter("@Wechat", DBNull.Value));
            }

            SqlParameter[] paramArr = parameters.ToArray();

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdateUser", paramArr);
            return count > 0;
        }

        public int GetUserCount()
        {
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "GetUserCount");
            return count;
        }

        public int GetUserCountByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "GetUserCount", param);
            return count;
        }
    }
}
