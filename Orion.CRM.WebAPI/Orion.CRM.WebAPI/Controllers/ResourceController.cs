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
    }
}