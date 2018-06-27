using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class TagAppService
    {
        private TagDataAdapter adapter = new TagDataAdapter();
        public int InsertTag(Entity.Tag tag)
        {
            return adapter.InsertTag(tag);
        }

        public bool UpdateTag(Entity.Tag tag)
        {
            return adapter.UpdateTag(tag);
        }

        public bool DeleteTag(int id)
        {
            return adapter.DeleteTag(id);
        }

        public IEnumerable<Entity.Tag> GetTagsByUserId(int userId)
        {
            return adapter.GetTagsByUserId(userId);
        }
    }
}
