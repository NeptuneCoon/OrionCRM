using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// 应用程序 "数据传输/转换/提取" 帮助类
    /// </summary>
    public class AppDTO
    {
        /// <summary>
        /// 从json配置文件中获取意向群
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetInclinationsFromJson(string webRoot)
        {
            var jsonPath = webRoot + @"\config\inclination.json";
            var inclinationJson = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<SelectItem>>(inclinationJson);

            return list;
        }

        /// <summary>
        /// 从json配置文件中获取资源状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetStatusFromJson(string webRoot)
        {
            var jsonPath = webRoot + @"\config\status.json";
            var inclinationJson = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<SelectItem>>(inclinationJson);

            return list;
        }

        /// <summary>
        /// 从json配置文件中获取洽谈次数
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetTalkCountFromJson(string webRoot)
        {
            var jsonPath = webRoot + @"\config\talkcount.json";
            var inclinationJson = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<SelectItem>>(inclinationJson);

            return list;
        }

        /// <summary>
        /// 从数据库中获取当前组织/公司下的角色
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="orgId">组织/公司Id</param>
        /// <returns></returns>
        public static List<Models.Role.Role> GetRoleListFromDb(string apiHost, int orgId)
        {
            string roleApiUrl = apiHost + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=10000&orgId=" + orgId;
            var roleList = APIInvoker.Get<List<Models.Role.Role>>(roleApiUrl);

            return roleList;
        }

        /// <summary>
        /// 从数据库中获取当前组织/公司下的项目
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="orgId">组织/公司Id</param>
        /// <returns></returns>
        public static List<Models.Project.Project> GetProjectsFromDb(string apiHost, int orgId)
        {
            string apiUrl = apiHost + "api/Project/GetProjectsByOrgId?orgId=" + orgId;
            var projects = APIInvoker.Get<List<Models.Project.Project>>(apiUrl);

            return projects;
        }

        /// <summary>
        /// 从数据库中获取项目下的业务组
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public static List<Models.Group.Group> GetGroupsFromDb(string apiHost, int projectId)
        {
            string apiUrl = apiHost + "api/Group/GetGroupsByProjectId?projectId=" + projectId;
            var groups = APIInvoker.Get<List<Models.Group.Group>>(apiUrl);

            return groups;
        }

        /// <summary>
        /// 从数据库中获取资源来源
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="orgId">组织/公司Id</param>
        /// <returns></returns>
        public static List<Models.Source.Source> GetSourcesFromDb(string apiHost, int orgId)
        {
            string apiUrl = apiHost + "api/ResourceSource/GetSourcesByOrgId?orgId=" + orgId;
            var sources = APIInvoker.Get<List<Models.Source.Source>>(apiUrl);

            return sources;
        }

        public static Dictionary<int,string> TalkWayCollection
        {
            get
            {
                return new Dictionary<int, string>() {
                    {1, "电话" },
                    {2, "网聊" },
                    {3, "面谈" },
                    {4, "Email" },
                    {5, "其他" }
                };
            }
            set
            {
                TalkWayCollection = value;
            }
        }

    }
}
