using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceAppService
    {
        private ResourceDataAdapter adapter = new ResourceDataAdapter();
        public int InsertResource(Entity.Resource resource)
        {
            return adapter.InsertResource(resource);
        }

        public bool UpdateResource(Entity.Resource resource)
        {
            return adapter.UpdateResource(resource);
        }

        public int InsertResourceProject(Entity.ResourceProject resourceProject)
        {
            return adapter.InsertResourceProject(resourceProject);
        }
    }
}
