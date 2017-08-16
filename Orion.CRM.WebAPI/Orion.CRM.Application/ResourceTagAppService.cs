using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceTagAppService
    {
        private ResourceTagDataAdapter adapter = new ResourceTagDataAdapter();
        public int InsertResourceTag(Entity.ResourceTag resourceTag)
        {
            return adapter.InsertResourceTag(resourceTag);
        }

        public bool DeleteResourceTag(int resourceId)
        {
            return adapter.DeleteResourceTag(resourceId);
        }
    }
}
