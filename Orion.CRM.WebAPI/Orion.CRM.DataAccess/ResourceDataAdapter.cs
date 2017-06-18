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

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("Resource", "InsertResource", paramArr);
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

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceSource", "UpdateResource", paramArr);
            return count > 0;
        }
        #endregion

        /// <summary>
        /// 分页获取资源
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        //public IEnumerable<Entity.Resource> GetResources(int pageIndex, int pageSize)
        //{
        //    SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceSource", "GetResources").Clone();
        //    mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());

        //    IEnumerable<Entity.Resource> resources = SqlMapHelper.GetSqlMapResult<Entity.Resource>(mapDetail);
        //    return resources;
        //}


        #region 添加资源和项目之间的关系
        public int InsertResourceProject(Entity.ResourceProject resourceProject)
        {
            if (resourceProject == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", resourceProject.ResourceId),
                new SqlParameter("@ProjectId", resourceProject.ProjectId),
                new SqlParameter("@CreateTime", resourceProject.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("Resource", "InsertResourceProject", paramArr);
            return identityId;
        }
        #endregion
    }
}
