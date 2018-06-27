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

        public bool DeleteProject(int id)
        {
            // 1.[CustomerSign]:delete from [CustomerSign] where ProjectId=@ProjectId;
            int count1 = new CustomerSignDataAdapter().DeleteSignByProjectId(id);

            // 2.删除Project下的所有[Group]<遍历>;调用删除业务组的代码](ResourceGroup,UserGroup,[Group])
            IEnumerable<Entity.Group> groups = new GroupDataAdapter().GetGroupsByProjectId(id);
            if (groups != null) {
                foreach(var group in groups) {
                    bool res = new GroupDataAdapter().DeleteGroup(group.Id);
                }
            }

            // 3.删除Project下的所有[Resource]<遍历>:调用删除Resource的代码(TalkRecord,ResourceTag,ResourceNote,ResourceUser,ResourceGroup,ResourceProject,ResourceOrg)
            IEnumerable<int> resourceIds = new ResourceDataAdapter().GetProjectResourceIds(id);
            if (resourceIds != null) {
                ResourceDataAdapter resourceAdapter = new ResourceDataAdapter();
                foreach(var resourceId in resourceIds) {
                    bool res = resourceAdapter.DeleteResource(resourceId);
                }
            }

            // 4.删除Project和用户的关系[UserProject]:delete from [UserProject] where ProjectId=@ProjectId
            int count2 = new AppUserDataAdapter().DeleteUserProjectByProjectId(id);
            // 5.删除Project自身

            int count3 = new ProjectDataAdapter().DeleteProject(id);

            return count3 > 0;
        }
    }
}
