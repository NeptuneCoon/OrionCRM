using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;
using System.Linq;

namespace Orion.CRM.Application
{
    public class TalkRecordAppService
    {
        private TalkRecordDataAdapter adapter = new TalkRecordDataAdapter();
        public int InsertTalkRecord(Entity.TalkRecord record)
        {
            // 添加洽谈记录
            int identityId = adapter.InsertTalkRecord(record);
            // 更新资源的最后联系时间(只有是用户添加的洽谈记录才会做此更新，资源分配等操作不更新最后联系时间和洽谈次数)
            if (identityId > 0 && record.Type == 0) {
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

        public IEnumerable<Entity.TalkcountRank> TalkRecordStat(int orgId, int projectId, int? groupId, string beginTime, string endTime)
        {
            var talkcountRanks = adapter.TalkRecordStat(orgId, projectId, groupId, beginTime, endTime);

            if (talkcountRanks != null) { 
                Dictionary<string, int> dict = new Dictionary<string, int>();
                
                foreach (var item in talkcountRanks) {
                    if (dict.ContainsKey(item.Saleman)) {
                        dict[item.Saleman] += item.Count;
                    }
                    else {
                        dict.Add(item.Saleman, item.Count);
                    }
                }
                
                dict.OrderByDescending(x => x.Value);
 
                List<Entity.TalkcountRank> rankRecords = new List<Entity.TalkcountRank>();
                decimal totalCount = dict.Sum(x => x.Value);
                foreach (var item in dict) {
                    Entity.TalkcountRank rank = new Entity.TalkcountRank();
                    rank.Saleman = item.Key;
                    rank.Count = item.Value;
                    rank.Percent = (item.Value / totalCount * 100).ToString("f1");

                    rankRecords.Add(rank);
                }
                return rankRecords;
            }
            return null;
        }

        public bool TalkRecordBatchInsert(IEnumerable<Entity.TalkRecordBatchInsert> talkRecords)
        {
            return adapter.TalkRecordBatchInsert(talkRecords);
        }
    }
}
