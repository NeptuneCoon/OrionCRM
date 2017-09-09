using Orion.CRM.Core;
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

        public IEnumerable<Entity.AppUser> GetUsersByCondition(Entity.AppUserSearchParams searchParam)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("AppUserDomain", "GetUsersByCondition").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", searchParam.pi.ToString()).Replace("$PageSize", searchParam.ps.ToString());

            string sqlWhere = "";
            if (!string.IsNullOrEmpty(searchParam.key)) {
                sqlWhere += $" and RealName like '%{searchParam.key}%'";
            }
            if (searchParam.gid != null && searchParam.gid > 0) {
                sqlWhere += $" and GroupId={searchParam.gid}";
            }
            if (searchParam.roleid != null && searchParam.roleid > 0) {
                sqlWhere += $" and RoleId={searchParam.roleid}";
            }
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            SqlParameter param = new SqlParameter("@OrgId", searchParam.oid);
            IEnumerable<Entity.AppUser> users = SqlMapHelper.GetSqlMapResult<Entity.AppUser>(mapDetail, param);
            return users;
        }

        public IEnumerable<Entity.AppUser> GetAllUsersByGroupId(int groupId)
        {
            SqlParameter param = new SqlParameter("@GroupId", groupId);
            IEnumerable<Entity.AppUser> users = SqlMapHelper.GetSqlMapResult<Entity.AppUser>("AppUserDomain", "GetAllUsersByGroupId", param);
            return users;
        }

        public IEnumerable<Entity.AppUserComplex> GetAllUsersByProjectId(int projectId)
        {
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            IEnumerable<Entity.AppUserComplex> users = SqlMapHelper.GetSqlMapResult<Entity.AppUserComplex>("AppUserDomain", "GetAllUsersByProjectId", param);
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

        public Entity.AppUser GetUserByEmail(string email)
        {
            SqlParameter param = new SqlParameter("@Email", email);
            Entity.AppUser appUser = SqlMapHelper.GetSqlMapSingleResult<Entity.AppUser>("AppUserDomain", "GetUserByEmail", param);

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
            parameters.Add(new SqlParameter("@Mobile", CheckNull(user.Mobile)));
            parameters.Add(new SqlParameter("@Email", CheckNull(user.Email)));
            parameters.Add(new SqlParameter("@Wechat", CheckNull(user.Wechat)));

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
            parameters.Add(new SqlParameter("@RealName", user.RealName));
            parameters.Add(new SqlParameter("@UpdateTime", user.UpdateTime));
            parameters.Add(new SqlParameter("@Enable", user.Enable));
            parameters.Add(new SqlParameter("@Mobile", CheckNull(user.Mobile)));
            parameters.Add(new SqlParameter("@Email", CheckNull(user.Email)));
            parameters.Add(new SqlParameter("@Wechat", CheckNull(user.Wechat)));

            SqlParameter[] paramArr = parameters.ToArray();

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdateUser", paramArr);
            return count > 0;
        }

        public bool UpdatePassword(string userId, string password)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", userId),
                new SqlParameter("@Password", password),
                new SqlParameter("@UpdateTime", DateTime.Now)
            };
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdatePassword", paramArr);
            return count > 0;
        }

        public int GetUserCount()
        {
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "GetUserCount");
            return count;
        }

        public int GetUserCountByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "GetUserCountByOrgId", param);
            return count;
        }

        public int GetUserCountByCondition(Entity.AppUserSearchParams searchParam)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("AppUserDomain", "GetUserCountByCondition").Clone();

            string sqlWhere = "";
            if (!string.IsNullOrEmpty(searchParam.key)) {
                sqlWhere += $" and RealName like '%{searchParam.key}%'";
            }
            if (searchParam.gid != null && searchParam.gid > 0) {
                sqlWhere += $" and GroupId={searchParam.gid}";
            }
            if (searchParam.roleid != null && searchParam.roleid > 0) {
                sqlWhere += $" and RoleId={searchParam.roleid}";
            }
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            SqlParameter param = new SqlParameter("@OrgId", searchParam.oid);

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>(mapDetail, param);
            return count;
        }

        public int InsertUserRole(Entity.UserRole userRole)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userRole.UserId),
                new SqlParameter("@RoleId", userRole.RoleId),
                new SqlParameter("@CreateTime", userRole.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "InsertUserRole", paramArr);
            return identityId;
        }

        public bool UpdateUserRole(Entity.UserRole userRole)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userRole.UserId),
                new SqlParameter("@RoleId", userRole.RoleId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdateUserRole", paramArr);
            return count > 0;
        }

        public Entity.UserProject GetUserProject(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            Entity.UserProject entity = SqlMapHelper.GetSqlMapSingleResult<Entity.UserProject>("AppUserDomain", "GetUserProject", param);
            return entity;
        }

        public int InsertUserProject(Entity.UserProject userProject)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userProject.UserId),
                new SqlParameter("@ProjectId", userProject.ProjectId),
                new SqlParameter("@CreateTime", userProject.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "InsertUserProject", paramArr);
            return identityId;
        }

        public bool UpdateUserProject(Entity.UserProject userProject)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userProject.UserId),
                new SqlParameter("@ProjectId", userProject.ProjectId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdateUserProject", paramArr);
            return count > 0;
        }

        public int DeleteUserProject(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "DeleteUserProject", param);
            return count;
        }

        public int DeleteUserProjectByProjectId(int projectId)
        {
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "DeleteUserProjectByProjectId", param);
            return count;
        }

        public Entity.UserGroup GetUserGroup(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            Entity.UserGroup entity = SqlMapHelper.GetSqlMapSingleResult<Entity.UserGroup>("AppUserDomain", "GetUserGroup", param);
            return entity;
        }

        public int InsertUserGroup(Entity.UserGroup userGroup)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userGroup.UserId),
                new SqlParameter("@GroupId", userGroup.GroupId),
                new SqlParameter("@CreateTime", userGroup.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("AppUserDomain", "InsertUserGroup", paramArr);
            return identityId;
        }

        public bool UpdateUserGroup(Entity.UserGroup userGroup)
        {
            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", userGroup.UserId),
                new SqlParameter("@GroupId", userGroup.GroupId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "UpdateUserGroup", paramArr);
            return count > 0;
        }

        public int DeleteUserGroup(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "DeleteUserGroup", param);
            return count;
        }

        // 删除用户及相关数据
        public int DeleteUser(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("AppUserDomain", "DeleteUser", param);
            return count;
        }
    }
}
