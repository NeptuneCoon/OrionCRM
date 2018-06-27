using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class CustomerSignAppService
    {
        private CustomerSignDataAdapter adapter = new CustomerSignDataAdapter();
        public int InsertSign(Entity.CustomerSign sign)
        {
            return adapter.InsertSign(sign);
        }

        public int Delete(int id)
        {
            return adapter.Delete(id);
        }

        public int DeleteSign(int resourceId)
        {
            return adapter.DeleteSign(resourceId);
        }

        public int DeleteSignByProjectId(int projectId)
        {
            return adapter.DeleteSignByProjectId(projectId);
        }

        public Entity.CustomerSign GetSignByResourceId(int resourceId)
        {
            return adapter.GetSignByResourceId(resourceId);
        }

        public IEnumerable<Entity.CustomerSign> GetSignsByTime(int orgId, string beginTime, string endTime)
        {
            return adapter.GetSignsByTime(orgId, beginTime, endTime);
        }

        public IEnumerable<Entity.CustomerSign> GetGroupMemberSigns(int groupId, string beginTime, string endTime)
        {
            return adapter.GetGroupMemberSigns(groupId, beginTime, endTime);
        }

        public IEnumerable<Entity.CustomerSign> GetProjectGroupSigns(int projectId, string beginTime, string endTime)
        {
            return adapter.GetProjectGroupSigns(projectId, beginTime, endTime);
        }

        public IEnumerable<Entity.CustomerSign> GetSignsByCondition(Entity.SignSearchParams param)
        {
            return adapter.GetSignsByCondition(param);
        }

        public int GetSignCountByCondition(Entity.SignSearchParams param)
        {
            return adapter.GetSignCountByCondition(param);
        }

        /// <summary>
        /// 获取某段时间内各组业绩
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity.GroupSaleRanking> GetGroupSaleRanking(int orgId, int projectId, string beginTime, string endTime)
        {
            return adapter.GetGroupSaleRanking(orgId, projectId, beginTime, endTime);
        }
    }
}
