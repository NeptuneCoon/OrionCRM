using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ProjectAppService
    {
        private ProjectDataAdapter adapter = new ProjectDataAdapter();
        public Entity.Project GetProjectById(int id)
        {
            return adapter.GetProjectById(id);
        }

        public IEnumerable<Entity.Project> GetProjectsByOrgId(int orgId)
        {
            return adapter.GetProjectsByOrgId(orgId);
        }

        public int InsertProject(Entity.Project project)
        {
            return adapter.InsertProject(project);
        }

        public bool UpdateProject(Entity.Project project)
        {
            return adapter.UpdateProject(project);
        }
    }
}
