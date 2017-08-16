using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class TagDataAdapter : DataAdapter
    {
        public int InsertTag(Entity.Tag tag)
        {
            if (tag == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@TagName", tag.TagName),
                new SqlParameter("@UserId", tag.UserId),
                new SqlParameter("@CreateTime", tag.CreateTime),
                new SqlParameter("@UpdateTime", tag.UpdateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("Tag", "InsertTag", paramArr);
            return identityId;
        }

        public bool UpdateTag(Entity.Tag tag)
        {
            if (tag == null || tag.Id <= 0) return false;


            SqlParameter[] paramArr = {
                new SqlParameter("@TagName", tag.TagName),
                new SqlParameter("@UpdateTime", tag.UpdateTime)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Tag", "UpdateTag", paramArr);
            return count > 0;
        }

        public bool DeleteTag(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("Tag", "DeleteTag", param);
            return count > 0;
        }

        public IEnumerable<Entity.Tag> GetTagsByUserId(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            IEnumerable<Entity.Tag> tags = SqlMapHelper.GetSqlMapResult<Entity.Tag>("Tag", "GetTagsByUserId", param);
            return tags;
        }
    }
}
