using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 业务组控制器
    /// </summary>
    public class GroupController : BaseController
    {
        public IActionResult List()
        {
            string url = _AppConfig.WebAPIHost + "api/Group/GetGroupsByOrgId?orgId=" + _AppUser.OrgId;
            List<Models.Group.Group> list = APIInvoker.Get<List<Models.Group.Group>>(url);

            ViewBag.ProjectId = _AppUser.ProjectId;//当前用户所属项目，如果不为空，则创建业务组时默认选中其所属的项目
            ViewBag.Projects = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);

            return View(list);
        }

        [HttpPost]
        public int Insert(Models.Group.Group group)
        {
            if (group != null) {
                group.ProjectId = group.ProjectId;
                group.CreateTime = DateTime.Now;
                group.UpdateTime = DateTime.Now;
                group.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebAPIHost + "api/Group/InsertGroup";
                int identityId = APIInvoker.Post<int>(url, group);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Group.Group group)
        {
            if (group != null && group.Id > 0) {
                string getUrl = _AppConfig.WebAPIHost + "api/Group/GetGroupById?id=" + group.Id;
                Models.Group.Group dbGroup = APIInvoker.Get<Models.Group.Group>(getUrl);
                if (dbGroup != null) {
                    dbGroup.GroupName = group.GroupName;
                    dbGroup.ProjectId = group.ProjectId;
                    dbGroup.UpdateTime = DateTime.Now;
                    dbGroup.ManagerId = group.ManagerId;
                    
                    string updateUrl = _AppConfig.WebAPIHost + "api/Group/UpdateGroup";
                    bool result = APIInvoker.Post<bool>(updateUrl, dbGroup);
                    return result;
                }
            }
            return false;
        }

        public bool Delete(int id)
        {
            if (id > 0) {
                string url = _AppConfig.WebAPIHost + "api/Group/DeleteGroup?id=" + id;
                bool result = APIInvoker.Get<bool>(url);
                return result;
            }
            return false;
        }

        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Group.Group> ReloadList()
        {
            string url = _AppConfig.WebAPIHost + "api/Group/GetGroupsByOrgId?orgId=" + _AppUser.OrgId;
            List<Models.Group.Group> list = APIInvoker.Get<List<Models.Group.Group>>(url);

            return list;
        }

        // 通用Ajax方法
        [HttpGet]
        public List<Models.Group.Group> GetGroupsByProjectId(int projectId)
        {
            return AppDTO.GetGroupsFromDb(_AppConfig.WebAPIHost, projectId);
        }

        [HttpPost]
        public bool SetGroupLeader(Models.Group.Group group)
        {
            if (group != null && group.Id > 0) {
                string getUrl = _AppConfig.WebAPIHost + "api/Group/GetGroupById?id=" + group.Id;
                Models.Group.Group dbGroup = APIInvoker.Get<Models.Group.Group>(getUrl);
                if (dbGroup != null) {
                    dbGroup.ManagerId = group.ManagerId;
                    dbGroup.UpdateTime = DateTime.Now;
                    
                    string updateUrl = _AppConfig.WebAPIHost + "api/Group/UpdateGroup";
                    bool result = APIInvoker.Post<bool>(updateUrl, dbGroup);
                    TempData["result"] = result;
                    return result;
                }
            }
            return false;
        }
    }
}