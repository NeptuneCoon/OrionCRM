using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Orion.CRM.DataAccess
{
    public class ResourceDataAdapter : DataAdapter
    {
        #region 添加资源
        public int InsertResource(Entity.Resource resource)
        {
            if (resource == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@CustomerName", resource.CustomerName.Trim()),
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
                new SqlParameter("@UpdateTime", resource.UpdateTime)
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
                new SqlParameter("@UpdateTime", resource.UpdateTime)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "UpdateResource", paramArr);
            return count > 0;
        }
        #endregion

        #region 删除一条资源
        public bool DeleteResource(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("Id", id);

            int result = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "DeleteResource", param);
            return result > 0;
        }
        #endregion

        #region 删除用户下的所有资源(仅删除资源关系)
        public int DeleteResourceUserByUserId(int userId)
        {
            if (userId <= 0) return 0;

            SqlParameter param = new SqlParameter("UserId", userId);

            int result = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "DeleteResourceUserByUserId", param);
            return result;
        } 
        #endregion

        #region 恢复一条资源
        public bool RestoreResource(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("Id", id);

            int result = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "RestoreResource", param);
            return result > 0;
        }
        #endregion

        #region 根据姓名/电话(Mobile/Tel)/微信/QQ查询一条资源
        /// <summary>
        /// 根据姓名/电话(Mobile/Tel)/微信/QQ查询一条资源
        /// </summary>
        /// <param name="key">查询关键词</param>
        /// <param name="orgId">组织机构Id</param>
        /// <returns></returns>
        public Entity.Resource GetResourceByNameMobileWechatQQ(string key, int orgId)
        {
            if (string.IsNullOrEmpty(key) || orgId <= 0) return null;

            SqlParameter[] paramArr = {
                new SqlParameter("OrgId", orgId),
                new SqlParameter("Key", key)
            };

            var result = SqlMapHelper.GetSqlMapSingleResult<Entity.Resource>("ResourceDomain", "GetResourceByNameMobileWechatQQ", paramArr);
            return result;
        } 
        #endregion

        #region 判断资源是否存在
        public bool IsResourceExist(int orgId, string mobile, string tel, string qq, string wechat)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "IsResourceExist").Clone();
            SqlParameter param = new SqlParameter("@OrgId", orgId);

            string sqlWhere = "";
            if (!string.IsNullOrEmpty(mobile)) {
                sqlWhere += $" and Mobile='{mobile}'";
            }
            if (!string.IsNullOrEmpty(tel)) {
                sqlWhere += $" and Tel='{tel}'";
            }
            if (!string.IsNullOrEmpty(qq)) {
                sqlWhere += $" and QQ='{qq}'";
            }
            if (!string.IsNullOrEmpty(wechat)) {
                sqlWhere += $" and Wechat='{wechat}'";
            }

            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>(mapDetail, param);
            return count > 0;
        }
        #endregion

        #region 设置资源状态
        public bool SetResourceStatus(int resourceId, int status)
        {
            if (resourceId <= 0 || status <= 0) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@Id", resourceId),
                new SqlParameter("@Status", status),
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "SetResourceStatus", paramArr);
            return count > 0;
        }
        #endregion

        #region 批量设置资源状态
        public int BatchSetResourceStatus(string resourceIds, int status)
        {
            if (string.IsNullOrEmpty(resourceIds)) return 0;

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "BatchSetResourceStatus").Clone();
            SqlParameter param = new SqlParameter("@Status", status);

            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$ResourceIds", resourceIds);

            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail, param);
            return count;
        }
        #endregion

        #region 删除指定资源和Group的关系
        public int DeleteResourceGroupByResourceIds(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return 0;

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "DeleteResourceGroupByResourceIds").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$ResourceIds", resourceIds);

            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail, null);
            return count;
        } 
        #endregion

        #region 添加资源和组织机构之间的关系
        public int InsertResourceOrganization(Entity.ResourceOrganization resourceOrg)
        {
            if (resourceOrg == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceOrg.ResourceId),
                new SqlParameter("@OrgId", resourceOrg.OrgId),
                new SqlParameter("@CreateTime", resourceOrg.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "InsertResourceOrganization", paramArr);
            return identityId;
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

        #region 根据Id获取资源
        public Entity.Resource GetResourceById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            Entity.Resource resource = SqlMapHelper.GetSqlMapSingleResult<Entity.Resource>("ResourceDomain", "GetResourceById", param);
            return resource;
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

        #region 获取未分配至业务组的资源
        public IEnumerable<Entity.UnassignedResource> GetGroupUnAssignedResources(int projectId)
        {
            if (projectId <= 0) return null;

            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            var resources = SqlMapHelper.GetSqlMapResult<Entity.UnassignedResource>("ResourceDomain", "GetGroupUnAssignedResources", param);
            return resources;
        }
        #endregion

        #region 获取未分配至业务组的资源个数
        public int GetGroupUnAssignedResourceCount(int projectId)
        {
            if (projectId <= 0) return 0;

            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "GetGroupUnAssignedResourceCount", param);
            return count;
        }
        #endregion

        #region 获取未分配至业务员的资源个数
        public int GetUserUnAssignedResourceCount(int orgId)
        {
            if (orgId <= 0) return 0;

            SqlParameter param = new SqlParameter("@OrgId", orgId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "GetUserUnAssignedResourceCount", param);
            return count;
        }
        #endregion

        #region 获取资源来源情况
        public IEnumerable<Entity.ResourceSource> GetResourceSourceStat(int orgId)
        {
            if (orgId <= 0) return null;
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            var list = SqlMapHelper.GetSqlMapResult<Entity.ResourceSource>("ResourceDomain", "GetResourceSourceStat", param);
            return list;
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
                    sb.Append($" and (GroupId>0 and UserId is null)");
                }
            }
            if (param.tagids != null && param.tagids.Count > 0) {
                string strTagIds = string.Join(",", param.tagids);
                sb.Append($" and TagId in({strTagIds})");
            }
            if (!string.IsNullOrEmpty(param.start)) {
                sb.Append($" and CreateTime >='{param.start}'");
            }
            if (!string.IsNullOrEmpty(param.end)) {
                sb.Append($" and CreateTime <='{param.end}'");
            }

            string sqlWhere = sb.ToString();
            return sqlWhere;
        }
        #endregion

        #region 获取某个资源来源下的资源数量
        public int GetResourceCountBySourceFrom(int orgId, int sourceId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrgId", orgId),
                new SqlParameter("@SourceFrom", sourceId)
            };

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "GetResourceCountBySourceFrom", parameters);
            return count;
        } 
        #endregion

        #region 设置某个来源下的所有资源的SourceFrom为空
        public int ClearSourceFrom(int sourceId)
        {
            SqlParameter param = new SqlParameter("@SourceFrom", sourceId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "ClearSourceFrom", param);
            return count;
        }
        #endregion

        #region 获取该用户下的资源个数
        public int GetResourceCountByUserId(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);

            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "GetResourceCountByUserId", param);
            return count;
        }
        #endregion

        #region 划分某一用户的资源到另一用户名下
        public int ChangeResourceUserOwner(int sourceUserId, int targetUserId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SourceUserId", sourceUserId),
                new SqlParameter("@TargetUserId", targetUserId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "ChangeResourceUserOwner", parameters);
            return count;
        }
        #endregion

        #region 划分某一用户的资源到另一用户所属组下
        public int ChangeResourceGroupOwner(int sourceUserId, int targetGroupId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SourceUserId", sourceUserId),
                new SqlParameter("@TargetGroupId", targetGroupId)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "ChangeResourceGroupOwner", parameters);
            return count;
        }
        #endregion

        public bool ResourceGroupBatchInsert(IEnumerable<Entity.ResourceGroup> resourceGroups)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.ResourceGroup>("ResourceDomain", "ResourceGroupBatchInsert", resourceGroups);
            return result;
        }

        public List<int> GetResourcesByUserId(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            List<int> resourceIds = SqlMapHelper.GetSqlMapResult<int>("ResourceDomain", "GetResourcesByUserId", param).ToList();
            return resourceIds;
        }

        public void AssignUserResourcesToPublic(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "AssignUserResourcesToPublic", param);
        }

        public void AssignUserResourcesToUnassigned(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "AssignUserResourcesToUnassigned", param);
        }

        // 获取资源属于哪个项目
        public Entity.ResourceProject GetResourceProject(int resourceId)
        {
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            var entity = SqlMapHelper.GetSqlMapSingleResult<Entity.ResourceProject>("ResourceDomain", "GetResourceProject", param);
            return entity;
        }

        public int GetTalkingResourceCountByUserId(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceDomain", "GetTalkingResourceCountByUserId", param);
            return count;
        }

        // 获取项目下所有资源Id
        public IEnumerable<int> GetProjectResourceIds(int projectId)
        {
            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            IEnumerable<int> resourceIds = SqlMapHelper.GetSqlMapResult<int>("ResourceDomain", "GetProjectResourceIds", param);

            return resourceIds;
        }

        public int BatchDeleteResourceGroup(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return 0;

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "BatchDeleteResourceGroup").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("@ResourceIds", resourceIds);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail);
            return count;
        }

        public int BatchDeleteResourceUser(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return 0;

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "BatchDeleteResourceUser").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("@ResourceIds", resourceIds);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail);
            return count;
        }

        // 更新资源的最后联系时间
        public bool UpdateLastTimeTalkCount(int resourceId, DateTime lastTime)
        {
            if (lastTime == null) return false;

            SqlParameter[] parameters = {
                 new SqlParameter("@LastTime", lastTime),
                 new SqlParameter("@Id", resourceId)
            };
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceDomain", "UpdateLastTimeTalkCount", parameters);
            return count > 0;
        }

        /// <summary>
        /// 批量更新ResourceProject(一般用于将一批资源从一个项目迁移到另一个项目下，这种操作比较少见)
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int UpdateResourceProjectByResourceIds(string resourceIds, int projectId)
        {
            if (string.IsNullOrEmpty(resourceIds) || projectId <= 0) return 0;

            SqlParameter param = new SqlParameter("@ProjectId", projectId);
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "UpdateResourceProjectByResourceIds").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$ResourceIds", resourceIds);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail, param);
            return count;
        }

        /// <summary>
        /// 批量更新ResourceGroup(一般用于将一个用户从一个组划分到另外一个组，此时他的资源应同时迁入该组)
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int UpdateResourceGroupByResourceIds(string resourceIds, int groupId)
        {
            if (string.IsNullOrEmpty(resourceIds) || groupId <= 0) return 0;

            SqlParameter param = new SqlParameter("@GroupId", groupId);
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceDomain", "UpdateResourceGroupByResourceIds").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$ResourceIds", resourceIds);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail, param);
            return count;
        }
    }
}
