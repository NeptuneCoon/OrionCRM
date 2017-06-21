using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class ResourceDataAdapter : DataAdapter
    {
        #region 添加资源
        public int InsertResource(Entity.Resource resource)
        {
            if (resource == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@CustomerName", resource.CustomerName),
                new SqlParameter("@Sex", CheckNull(resource.Sex)),
                new SqlParameter("@Address", CheckNull(resource.Address)),
                new SqlParameter("@MsgTime", CheckNull(resource.MsgTime)),
                new SqlParameter("@LastTime", CheckNull(resource.LastTime)),
                new SqlParameter("@SourceFrom", CheckNull(resource.SourceFrom)),
                new SqlParameter("@Status", CheckNull(resource.Status)),
                new SqlParameter("@Inclination", CheckNull(resource.Inclination)),
                new SqlParameter("@TalkCount", CheckNull(resource.TalkCount)),
                new SqlParameter("@Message", CheckNull(resource.Message)),
                new SqlParameter("@Mobile", CheckNull(resource.Mobile)),
                new SqlParameter("@Tel", CheckNull(resource.Tel)),
                new SqlParameter("@QQ", CheckNull(resource.QQ)),
                new SqlParameter("@Wechat", CheckNull(resource.Wechat)),
                new SqlParameter("@Email", CheckNull(resource.Email)),
                new SqlParameter("@Remark", CheckNull(resource.Remark)),
                new SqlParameter("@InvalidReason", CheckNull(resource.InvalidReason)),
                new SqlParameter("@AppendUserId", resource.AppendUserId),
                new SqlParameter("@CreateTime", resource.CreateTime),
                new SqlParameter("@UpdateTime", resource.UpdateTime),
                new SqlParameter("@DeleteFlag", resource.DeleteFlag)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "InsertResource", paramArr);
            return identityId;
        }
        #endregion

        #region 修改资源
        public bool UpdateResource(Entity.Resource resource)
        {
            if (resource == null || resource.Id <= 0) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@Id", resource.Id),
                new SqlParameter("@CustomerName", resource.CustomerName),
                new SqlParameter("@TalkCount", CheckNull(resource.TalkCount)),
                new SqlParameter("@UpdateTime", resource.UpdateTime),
                new SqlParameter("@DeleteFlag", resource.DeleteFlag)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "UpdateResource", paramArr);
            return count > 0;
        }
        #endregion

        #region 添加资源和项目之间的关系
        public int InsertResourceProject(Entity.ResourceProject resourceProject)
        {
            if (resourceProject == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceProject.ResourceId),
                new SqlParameter("@ProjectId", resourceProject.ProjectId),
                new SqlParameter("@CreateTime", resourceProject.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "InsertResourceProject", paramArr);
            return identityId;
        }
        #endregion

        #region 根据筛选条件查询资源
        /// <summary>
        /// 根据筛选条件查询资源
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        public IEnumerable<Entity.Resource> GetResourcesByCondition(Entity.ResourceSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "GetResourcesByCondition").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", param.pi.ToString()).Replace("$PageSize", param.ps.ToString());

            string sqlWhere = GetSqlWhere(param);
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            IEnumerable<Entity.Resource> resources = SqlMapHelper.GetSqlMapResult<Entity.Resource>(mapDetail);
            return resources;
        }
        #endregion

        #region 根据筛选条件获取符合条件的资源个数
        /// <summary>
        /// 根据筛选条件获取符合条件的资源个数
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        public int GetResourceCountByCondition(Entity.ResourceSearchParams param)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "GetResourceCountByCondition").Clone();

            string sqlWhere = GetSqlWhere(param);
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>(mapDetail);
            return count;
        }
        #endregion

        #region 添加资源和业务组之间的关系
        public int InsertResourceGroup(Entity.ResourceGroup resourceGroup)
        {
            if (resourceGroup == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceGroup.ResourceId),
                new SqlParameter("@GroupId", resourceGroup.GroupId),
                new SqlParameter("@CreateTime", resourceGroup.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "InsertResourceGroup", paramArr);
            return identityId;
        }
        #endregion

        #region 修改资源和业务组之间的关系
        public bool UpdateResourceGroup(Entity.ResourceGroup resourceGroup)
        {
            if (resourceGroup == null) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceGroup.ResourceId),
                new SqlParameter("@GroupId", resourceGroup.GroupId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "UpdateResourceGroup", paramArr);
            return count > 0;
        }
        #endregion

        #region 获取资源和业务组之间的关系
        public Entity.ResourceGroup GetResourceGroup(int resourceId)
        {
            if (resourceId <= 0) return null;

            SqlParameter param = new SqlParameter("@ResourceId", resourceId);

            var resourceGroup = SqlMapHelper.GetSqlMapSingleResult<Entity.ResourceGroup>("ResourceDomain", "GetResourceGroup", param);
            return resourceGroup;
        }
        #endregion

        #region 添加资源和用户之间的关系
        public int InsertResourceUser(Entity.ResourceUser resourceUser)
        {
            if (resourceUser == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceUser.ResourceId),
                new SqlParameter("@UserId", resourceUser.UserId),
                new SqlParameter("@CreateTime", resourceUser.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "InsertResourceUser", paramArr);
            return identityId;
        }
        #endregion

        #region 修改资源和用户之间的关系
        public bool UpdateResourceUser(Entity.ResourceUser resourceUser)
        {
            if (resourceUser == null) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceUser.ResourceId),
                new SqlParameter("@UserId", resourceUser.UserId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "UpdateResourceUser", paramArr);
            return count > 0;
        }
        #endregion

        #region 获取资源和用户之间的关系
        public Entity.ResourceUser GetResourceUser(int resourceId)
        {
            if (resourceId <= 0) return null;

            SqlParameter param = new SqlParameter("@ResourceId", resourceId);

            var resourceGroup = SqlMapHelper.GetSqlMapSingleResult<Entity.ResourceUser>("ResourceDomain", "GetResourceUser", param);
            return resourceGroup;
        }
        #endregion


        #region 生成SQL查询的where子句
        private string GetSqlWhere(Entity.ResourceSearchParams param)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(param.name)) {
                sb.Append($" and CustomerName like '%{param.name}%'");
            }
            if (!string.IsNullOrEmpty(param.key)) {
                sb.Append($" and (Mobile='{param.key}' or QQ='{param.key}' or Wechat='{param.key}')");
            }
            if (param.pid != null && param.pid > 0) {
                sb.Append($" and ProjectId={param.pid}");
            }
            if (param.gid != null && param.gid > 0) {
                sb.Append($" and GroupId={param.gid}");
            }
            if (param.uid != null && param.uid > 0) {
                sb.Append($" and UserId={param.uid}");
            }
            if (param.status != null && param.status > 0) {
                sb.Append($" and Status={param.status}");
            }
            if (param.talk != null && param.talk > 0) {
                sb.Append($" and TalkCount={param.talk}");
            }
            if (param.inc != null && param.inc > 0) {
                sb.Append($" and Inclination={param.inc}");
            }
            if (param.source != null && param.source > 0) {
                sb.Append($" and SourceFrom={param.source}");
            }
            if (param.assign != null && param.assign > 0) {
                if (param.assign == 1) {
                    // 客服未分
                    sb.Append($" and GroupId is null");
                }
                else if (param.assign == 2) {
                    // 组长未分
                    sb.Append($" and UserId is null");
                }
            }

            string sqlWhere = sb.ToString();
            return sqlWhere;
        } 
        #endregion
    }
}
