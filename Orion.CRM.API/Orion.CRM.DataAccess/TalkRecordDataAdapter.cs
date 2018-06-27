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

        public bool DeleteTalkRecord(int id, int resourceId)
        {
            if (id <= 0) return false;

            SqlParameter[] parameters = {
                new SqlParameter("@Id", id),
                new SqlParameter("@ResourceId", resourceId)
            };
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("TalkRecord", "DeleteTalkRecord", parameters);

            return count > 0;
        }

        public IEnumerable<Entity.TalkRecord> GetRecordsByResourceId(int resourceId)
        {
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            var talkRecords = SqlMapHelper.GetSqlMapResult<Entity.TalkRecord>("TalkRecord", "GetRecordsByResourceId", param);

            return talkRecords;
        }

        public IEnumerable<Entity.TalkcountRank> TalkRecordStat(int orgId, int projectId, int? groupId, string beginTime, string endTime)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@OrgId", orgId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };


            string sqlWhere = "";
            if (groupId != null && groupId > 0) {
                sqlWhere = "and C.GroupId=" + groupId;
            }

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("TalkRecord", "TalkRecordStat").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$SqlWhere", sqlWhere);

            var queryResult = SqlMapHelper.GetSqlMapResult<Entity.TalkcountRank>(mapDetail, parameters);
            return queryResult;
        }

        public bool TalkRecordBatchInsert(IEnumerable<Entity.TalkRecordBatchInsert> talkRecords)
        {
            try { 
                bool result = SqlMapHelper.ExecuteBatchInsert<Entity.TalkRecordBatchInsert>("TalkRecord", "TalkRecordBatchInsert", talkRecords);
                return result;
            }
            catch {
                return false;
            }
        }
    }
}
