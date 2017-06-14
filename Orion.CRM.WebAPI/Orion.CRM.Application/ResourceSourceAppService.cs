using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceSourceAppService
    {
        private ResourceSourceAdapter adapter = new ResourceSourceAdapter();
        public int InsertSource(Entity.ResourceSource source)
        {
            return adapter.InsertSource(source);
        }

        public bool UpdateSource(Entity.ResourceSource source)
        {
            return adapter.UpdateSource(source);
        }

        public bool DeleteSource(int id)
        {
            return adapter.DeleteSource(id);
        }

        public IEnumerable<Entity.ResourceSource> GetSourcesByOrgId(int orgId)
        {
            return adapter.GetSourcesByOrgId(orgId);
        }
    }
}
