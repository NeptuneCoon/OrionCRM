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

        public int DeleteSign(int resourceId)
        {
            return adapter.DeleteSign(resourceId);
        }

        public Entity.CustomerSign GetSignByResourceId(int resourceId)
        {
            return adapter.GetSignByResourceId(resourceId);
        }

        public IEnumerable<Entity.CustomerSign> GetSignsByTime(int orgId, string beginTime, string endTime)
        {
            return adapter.GetSignsByTime(orgId, beginTime, endTime);
        }
    }
}
