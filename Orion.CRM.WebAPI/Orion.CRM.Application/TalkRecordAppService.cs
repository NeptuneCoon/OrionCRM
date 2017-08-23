using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class TalkRecordAppService
    {
        private TalkRecordDataAdapter adapter = new TalkRecordDataAdapter();
        public int InsertTalkRecord(Entity.TalkRecord record)
        {
            // 添加洽谈记录
            int identityId = adapter.InsertTalkRecord(record);
            // 更新资源的最后联系时间
            if (identityId > 0) {
                bool result = new ResourceDataAdapter().UpdateLastTimeTalkCount(record.ResourceId, DateTime.Now);
            }

            return identityId;
        }

        public bool DeleteTalkRecord(int id)
        {
            return adapter.DeleteTalkRecord(id);
        }

        public IEnumerable<Entity.TalkRecord> GetRecordsByResourceId(int resourceId)
        {
            return adapter.GetRecordsByResourceId(resourceId);
        }
    }
}
