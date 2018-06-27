using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class OrganizationAppService
    {
        private OrganizationDataAdapter adapter = new OrganizationDataAdapter();

        public Entity.Organization GetOrganizationById(int id)
        {
            return adapter.GetOrganizationById(id);
        }

        public IEnumerable<Entity.Organization> GetAllOrganizations()
        {
            return adapter.GetAllOrganizations();
        }

        public int InsertOrganization(Entity.Organization org)
        {
            return adapter.InsertOrganization(org);
        }

        public bool UpdateOrganization(Entity.Organization org)
        {
            return adapter.UpdateOrganization(org);
        }
    }
}
