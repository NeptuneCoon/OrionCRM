using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.Application;

namespace Orion.CRM.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ResourceController : Controller
    {
        private ResourceAppService service = new ResourceAppService();

        [HttpPost]
        public APIDataResult InsertResource([FromBody]Entity.Resource resource)
        {
            try {
                int identityId = service.InsertResource(resource);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateReource([FromBody]Entity.Resource resource)
        {
            try {
                bool res = service.UpdateResource(resource);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        // ��ɾ��һ����Դ
        public APIDataResult DeleteResource(int id)
        {
            try {
                bool res = service.DeleteResource(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ɾ��ResourceUser��¼
        public APIDataResult DeleteResourceUserByUserId(int userId)
        {
            try {
                int count = service.DeleteResourceUserByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �ָ�һ����Դ
        public APIDataResult RestoreResource(int id)
        {
            try {
                bool res = service.RestoreResource(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertResourceOrganization([FromBody]Entity.ResourceOrganization resourceOrg)
        {
            try {
                int identityId = service.InsertResourceOrganization(resourceOrg);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertResourceProject([FromBody]Entity.ResourceProject resourceProject)
        {
            try {
                int identityId = service.InsertResourceProject(resourceProject);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// ����ɸѡ������ѯ��Դ
        /// </summary>
        /// <param name="param">ɸѡ��������</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetResourcesByCondition([FromBody]Entity.ResourceSearchParams param)
        {
            try {
                IEnumerable<Entity.Resource> resources = service.GetResourcesByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, resources);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// ����ɸѡ������ȡ������������Դ����
        /// </summary>
        /// <param name="param">ɸѡ��������</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetResourceCountByCondition([FromBody]Entity.ResourceSearchParams param)
        {
            try {
                int count = service.GetResourceCountByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����Id��ȡ��Դ
        public APIDataResult GetResourceById(int id)
        {
            try {
                Entity.Resource resource = service.GetResourceById(id);
                APIDataResult dataResult = new APIDataResult(200, resource);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// ��������/�绰(Mobile/Tel)/΢��/QQ��ѯһ����Դ
        /// </summary>
        /// <param name="key">��ѯ�ؼ���</param>
        /// <param name="orgId">��֯����Id</param>
        /// <returns></returns>
        public APIDataResult GetResourceByNameMobileWechatQQ(string key, int orgId)
        {
            try {
                Entity.Resource resource = service.GetResourceByNameMobileWechatQQ(key, orgId);
                APIDataResult dataResult = new APIDataResult(200, resource);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �ж���Դ�Ƿ����
        public APIDataResult IsResourceExist(int orgId, string mobile, string tel, string qq, string wechat)
        {
            try {
                bool result = service.IsResourceExist(orgId, mobile, tel, qq, wechat);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ������Դ״̬
        public APIDataResult SetResourceStatus(int resourceId, int status)
        {
            try {
                bool result = service.SetResourceStatus(resourceId, status);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����������Դ״̬
        public APIDataResult BatchSetResourceStatus(string resourceIds, int status)
        {
            try {
                int count = service.BatchSetResourceStatus(resourceIds, status);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ɾ��ָ����Դ��Group�Ĺ�ϵ
        public APIDataResult DeleteResourceGroupByResourceIds(string resourceIds)
        {
            try {
                int count = service.DeleteResourceGroupByResourceIds(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �����Դ��ҵ����֮��Ĺ�ϵ
        [HttpPost]
        public APIDataResult InsertResourceGroup([FromBody]Entity.ResourceGroup resourceGroup)
        {
            try {
                int identityId = service.InsertResourceGroup(resourceGroup);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �޸���Դ��ҵ����֮��Ĺ�ϵ
        [HttpPost]
        public APIDataResult UpdateResourceGroup([FromBody]Entity.ResourceGroup resourceGroup)
        {
            try {
                bool res = service.UpdateResourceGroup(resourceGroup);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ��Դ��ҵ����֮��Ĺ�ϵ
        public APIDataResult GetResourceGroup(int resourceId)
        {
            try {
                Entity.ResourceGroup resourceGroup = service.GetResourceGroup(resourceId);
                APIDataResult dataResult = new APIDataResult(200, resourceGroup);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �����Դ���û�֮��Ĺ�ϵ
        [HttpPost]
        public APIDataResult InsertResourceUser([FromBody]Entity.ResourceUser resourceUser)
        {
            try {
                int identityId = service.InsertResourceUser(resourceUser);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // �޸���Դ���û�֮��Ĺ�ϵ
        [HttpPost]
        public APIDataResult UpdateResourceUser([FromBody]Entity.ResourceUser resourceUser)
        {
            try {
                bool res = service.UpdateResourceUser(resourceUser);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ��Դ���û�֮��Ĺ�ϵ
        public APIDataResult GetResourceUser(int resourceId)
        {
            try {
                Entity.ResourceUser resourceUser = service.GetResourceUser(resourceId);
                APIDataResult dataResult = new APIDataResult(200, resourceUser);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡδ������ҵ�������Դ
        public APIDataResult GetGroupUnAssignedResources(int projectId)
        {
            try {
                IEnumerable<Entity.UnassignedResource> resourceIds = service.GetGroupUnAssignedResources(projectId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡδ������ҵ�������Դ����
        public APIDataResult GetGroupUnAssignedResourceCount(int projectId)
        {
            try {
                int count = service.GetGroupUnAssignedResourceCount(projectId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡδ������ҵ��Ա����Դ����
        public APIDataResult GetUserUnAssignedResourceCount(int orgId)
        {
            try {
                int count = service.GetUserUnAssignedResourceCount(orgId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ��Դ��Դ���
        public APIDataResult GetResourceSourceStat(int orgId)
        {
            try {
                IEnumerable<Entity.ResourceSource> query = service.GetResourceSourceStat(orgId);
                APIDataResult dataResult = new APIDataResult(200, query);
                return dataResult;
            }
            catch(Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡĳ����Դ��Դ�µ���Դ����
        public APIDataResult GetResourceCountBySourceFrom(int orgId, int sourceId)
        {
            try {
                int count = service.GetResourceCountBySourceFrom(orgId, sourceId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����ĳ����Դ�µ�������Դ��SourceFromΪ��
        public APIDataResult ClearSourceFrom(int sourceId)
        {
            try {
                int count = service.ClearSourceFrom(sourceId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ���û��µ���Դ����
        public APIDataResult GetResourceCountByUserId(int userId)
        {
            try {
                int count = service.GetResourceCountByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����ĳһ�û�����Դ����һ�û�����
        public APIDataResult ChangeResourceUserOwner(int sourceUserId, int targetUserId)
        {
            try {
                int count = service.ChangeResourceUserOwner(sourceUserId, targetUserId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����ĳһ�û�����Դ����һ�û���������
        public APIDataResult ChangeResourceGroupOwner(int sourceUserId, int targetGroupId)
        {
            try {
                int count = service.ChangeResourceGroupOwner(sourceUserId, targetGroupId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult ResourceGroupBatchInsert([FromBody]IEnumerable<Entity.ResourceGroup> resourceGroups)
        {
            try {
                bool result = service.ResourceGroupBatchInsert(resourceGroups);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetResourcesByUserId(int userId)
        {
            try {
                List<int> resourceIds = service.GetResourcesByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        /// <summary>
        /// ���û�����Դ���빫��
        /// </summary>
        /// <param name="userId"></param>
        public APIDataResult AssignUserResourcesToPublic(int userId)
        {
            try {
                service.AssignUserResourcesToPublic(userId);
                APIDataResult dataResult = new APIDataResult(200, string.Empty);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// ���û�����Դ����δ����
        /// </summary>
        /// <param name="userId"></param>
        public APIDataResult AssignUserResourcesToUnassigned(int userId)
        {
            try {
                service.AssignUserResourcesToUnassigned(userId);
                APIDataResult dataResult = new APIDataResult(200, string.Empty);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ��Դ�����ĸ���Ŀ
        public APIDataResult GetResourceProject(int resourceId)
        {
            try {
                var entity = service.GetResourceProject(resourceId);
                APIDataResult dataResult = new APIDataResult(200, entity);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ�û�Ǣ̸�е���Դ������(Ǣ̸�е�)
        public APIDataResult GetTalkingResourceCountByUserId(int userId)
        {
            try {
                int count = service.GetTalkingResourceCountByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ȡ��Ŀ��������ԴId
        public APIDataResult GetProjectResourceIds(int projectId)
        {
            try {
                IEnumerable<int> resourceIds = service.GetProjectResourceIds(projectId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// ��Դ��������
        /// </summary>
        /// <returns></returns>
        public APIDataResult ResourceBatchAssign(string resourceIds, int groupId, int userId, int operatorId)
        {
            try {
                bool result = service.ResourceBatchAssign(resourceIds, groupId, userId, operatorId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ����ResourceId��������ɾ��ResourceUser
        public APIDataResult BatchDeleteResourceUser(string resourceIds)
        {
            try {
                int count = service.BatchDeleteResourceUser(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // ��ĳһ��Դ���ָ��Լ�
        public APIDataResult DivideToMe(int resourceId, int groupId, int userId)
        {
            try {
                bool result = service.DivideToMe(resourceId, groupId, userId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}