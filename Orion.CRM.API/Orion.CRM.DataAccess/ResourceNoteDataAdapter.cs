using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class ResourceNoteDataAdapter : DataAdapter
    {
        public int InsertResourceNote(Entity.ResourceNote note)
        {
            if (note == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@ResourceId",note.ResourceId),
                new SqlParameter("@Message",note.Message),
                new SqlParameter("@IsRemind",note.IsRemind),
                new SqlParameter("@RemindTime",note.RemindTime),
                new SqlParameter("@UserId", note.UserId),
                new SqlParameter("@CreateTime", note.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceNote", "InsertResourceNote", paramArr);
            return identityId;
        }

        public bool DeleteResourceNote(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceNote", "DeleteResourceNote", param);

            return count > 0;
        }

        public IEnumerable<Entity.ResourceNote> GetNotesByResourceId(int resourceId)
        {
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            var notes = SqlMapHelper.GetSqlMapResult<Entity.ResourceNote>("ResourceNote", "GetNotesByResourceId", param);

            return notes;
        }
    }
}
