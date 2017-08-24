using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class TalkRecordDataAdapter : DataAdapter
    {
        public int InsertTalkRecord(Entity.TalkRecord record)
        {
            if (record == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId", record.ResourceId),
                new SqlParameter("@TalkWay", record.TalkWay),
                new SqlParameter("@TalkResult", record.TalkResult),
                new SqlParameter("@UserId", record.UserId),
                new SqlParameter("@Type", record.Type),
                new SqlParameter("@CreateTime", record.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("TalkRecord", "InsertTalkRecord", paramArr);
            return identityId;
        }

        public bool DeleteTalkRecord(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("TalkRecord", "DeleteTalkRecord", param);

            return count > 0;
        }

        public IEnumerable<Entity.TalkRecord> GetRecordsByResourceId(int resourceId)
        {
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            var talkRecords = SqlMapHelper.GetSqlMapResult<Entity.TalkRecord>("TalkRecord", "GetRecordsByResourceId", param);

            return talkRecords;
        }

        public bool TalkRecordBatchInsert(IEnumerable<Entity.TalkRecordBatchInsert> talkRecords)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.TalkRecordBatchInsert>("TalkRecord", "TalkRecordBatchInsert", talkRecords);
            return result;
        }
    }
}
