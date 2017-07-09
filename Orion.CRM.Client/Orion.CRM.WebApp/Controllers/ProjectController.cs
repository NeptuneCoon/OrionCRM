using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Microsoft.Extensions.Options;
using Orion.CRM.WebTools;
using Newtonsoft.Json;

namespace Orion.CRM.WebApp.Controllers
{
    public class ProjectController : BaseController
    {
  
        public IActionResult List()
        {
            string url = _AppConfig.WebApiHost + "api/Project/GetProjectsByOrgId?orgId="+ _AppUser.OrgId;
            List<Models.Project.ProjectViewModel> list = APIInvoker.Get<List<Models.Project.ProjectViewModel>>(url);
            if(list != null && list.Count > 0) {
                foreach(var project in list) {
                    project.CreateUserName = GetUserNameById(project.CreateUserId);
                }
            }

            return View(list);
        }

        [HttpPost]
        public int Insert(Models.Project.ProjectViewModel project)
        {
            if (project != null) {
                project.ProjectName = project.ProjectName.Trim();
                project.OrgId = _AppUser.OrgId;
                project.CreateTime = DateTime.Now;
                project.UpdateTime = DateTime.Now;
                project.CreateUserId = _AppUser.Id;

                string url = _AppConfig.WebApiHost + "api/Project/InsertProject";
                int identityId = APIInvoker.Post<int>(url, project);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Project.ProjectViewModel project)
        {
            if (project != null && project.Id > 0) {
                project.ProjectName = project.ProjectName.Trim();
                project.UpdateTime = DateTime.Now;
                project.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebApiHost + "api/Project/UpdateProject";
                bool result = APIInvoker.Post<bool>(url, project);
                return result;
            }
            return false;
        }

        public bool Delete(int id)
        {
            if (id > 0) {
                string url = _AppConfig.WebApiHost + "api/Project/DeleteProject?id=" + id;
                bool result = APIInvoker.Get<bool>(url);
                return result;
            }
            return false;
        }

        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Project.ProjectViewModel> ReloadList()
        {
            string url = _AppConfig.WebApiHost + "api/Project/GetProjectsByOrgId?orgId=" + _AppUser.OrgId;
            List<Models.Project.ProjectViewModel> list = APIInvoker.Get<List<Models.Project.ProjectViewModel>>(url);
            if (list != null && list.Count > 0) {
                foreach (var project in list) {
                    project.CreateUserName = GetUserNameById(project.CreateUserId);
                }
            }

            return list;
        }

        #region 根据用户Id获取用户登录名
        private string GetUserNameById(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + userId;
            Models.Account.AppUserModel appUser = APIInvoker.Get<Models.Account.AppUserModel>(apiUrl);
            if (appUser != null) {
                return appUser.UserName;
            }
            return string.Empty;
        }
        #endregion
    }
}