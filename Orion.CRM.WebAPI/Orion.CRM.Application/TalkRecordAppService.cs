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
            return adapter.InsertTalkRecord(record);
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
