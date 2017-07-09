using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceSourceAppService
    {
        private ResourceSourceAdapter adapter = new ResourceSourceAdapter();
        public int InsertSource(Entity.Source source)
        {
            return adapter.InsertSource(source);
        }

        public bool UpdateSource(Entity.Source source)
        {
            return adapter.UpdateSource(source);
        }

        public bool DeleteSource(int id)
        {
            return adapter.DeleteSource(id);
        }

        public IEnumerable<Entity.Source> GetSourcesByOrgId(int orgId)
        {
            return adapter.GetSourcesByOrgId(orgId);
        }
    }
}
