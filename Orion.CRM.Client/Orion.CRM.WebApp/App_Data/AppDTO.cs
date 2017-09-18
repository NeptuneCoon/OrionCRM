using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// 读取配置文件
        /// </summary>
        /// <param name="key">配置节点，多级请以:分隔</param>
        /// <returns></returns>
        static string GetConfigurationSettings(string key)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            IConfigurationRoot Configuration = builder.Build();
            string value = Configuration[key];
            return value;
        }

        /// <summary>
        /// 从json配置文件中获取意向群
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetInclinationsFromJson()
        {
            var jsonPath = Directory.GetCurrentDirectory() + @"\wwwroot\config\inclination.json";
            var inclinationJson = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<SelectItem>>(inclinationJson);

            return list;
        }

        /// <summary>
        /// 从json配置文件中获取资源状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetStatusFromJson()
        {
            var jsonPath = Directory.GetCurrentDirectory() + @"\wwwroot\config\status.json";
            var inclinationJson = System.IO.File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<SelectItem>>(inclinationJson);

            return list;
        }

        /// <summary>
        /// 从json配置文件中获取洽谈次数
        /// </summary>
        /// <returns></returns>
        public static List<SelectItem> GetTalkCountFromJson()
        {
            var jsonPath = Directory.GetCurrentDirectory() + @"\wwwroot\config\talkcount.json";
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
        public static List<Models.Role.Role> GetRoleListFromDb(int orgId)
        {
            string roleApiUrl = GetConfigurationSettings("WebApiHost") + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=10000&orgId=" + orgId;
            var roleList = APIInvoker.Get<List<Models.Role.Role>>(roleApiUrl);

            return roleList;
        }

        /// <summary>
        /// 从数据库中获取当前组织/公司下的项目
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="orgId">组织/公司Id</param>
        /// <returns></returns>
        public static List<Models.Project.Project> GetProjectsFromDb(int orgId)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/Project/GetProjectsByOrgId?orgId=" + orgId;
            var projects = APIInvoker.Get<List<Models.Project.Project>>(apiUrl);

            return projects;
        }

        /// <summary>
        /// 根据项目Id,从数据库中获取项目下的业务组
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public static List<Models.Group.Group> GetGroupsByProjectId(int projectId)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/Group/GetGroupsByProjectId?projectId=" + projectId;
            var groups = APIInvoker.Get<List<Models.Group.Group>>(apiUrl);

            return groups;
        }

        /// <summary>
        /// 根据组织机构Id,从数据库中获取所有业务组
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<Models.Group.Group> GetGroupsByOrgId(int orgId)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/Group/GetGroupsByOrgId?orgId=" + orgId;
            var groups = APIInvoker.Get<List<Models.Group.Group>>(apiUrl);
            return groups;
        }

        /// <summary>
        /// 根据业务组Id获取组成员
        /// </summary>
        /// <param name="groupId">业务组Id</param>
        /// <returns></returns>
        public static List<Models.AppUser.AppUserViewModel> GetUsersByGroupId(int groupId)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/AppUser/GetAllUsersByGroupId?groupId=" + groupId;
            List<Models.AppUser.AppUserViewModel> users = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUrl);
            return users;
        }

        /// <summary>
        /// 从数据库中获取资源来源
        /// </summary>
        /// <param name="apiHost">API基地址</param>
        /// <param name="orgId">组织/公司Id</param>
        /// <returns></returns>
        public static List<Models.Source.Source> GetSourcesFromDb(int orgId)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/Source/GetSourcesByOrgId?orgId=" + orgId;
            var sources = APIInvoker.Get<List<Models.Source.Source>>(apiUrl);

            return sources;
        }

        // 洽谈方式字典
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

        /// <summary>
        /// 根据资源状态Id获取状态的展示名称
        /// </summary>
        /// <param name="status"></param>
        public static string GetStatusDisplayText(int? status)
        {
            if (status == null) return string.Empty;

            List<SelectItem> items = GetStatusFromJson();
            var query = items.FirstOrDefault(x => x.value == status);
            if (query != null) {
                return query.displayText;
            }
            return "";
        }

        /// <summary>
        /// 根据意向Id获取意向的展示名称
        /// </summary>
        /// <param name="inclination"></param>
        /// <returns></returns>
        public static string GetInclinationDisplayText(int? inclination)
        {
            if (inclination == null) return string.Empty;

            List<SelectItem> items = GetInclinationsFromJson();
            var query = items.FirstOrDefault(x => x.value == inclination);
            if (query != null) {
                return query.displayText;
            }
            return "";
        }

        /// <summary>
        /// 根据资源来源Id获取来源展示名称
        /// </summary>
        /// <param name="sourceFrom"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static string GetSourceDisplayText(int? sourceFrom, int orgId)
        {
            if (sourceFrom == null) return string.Empty;

            List<Models.Source.Source> items = GetSourcesFromDb(orgId);
            var query = items.FirstOrDefault(x => x.Id == sourceFrom);
            if (query != null) {
                return query.SourceName;
            }
            return "";
        }

        /// <summary>
        /// 根据性别value值获得展示名称
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static string GetSexDisplayText(int? sex)
        {
            if (sex == null) return string.Empty;

            if (sex == 1) return "男";
            else if (sex == 2) return "女";
            else return string.Empty;
        }


        /// <summary>
        /// 根据Email获取用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Models.AppUser.AppUserViewModel GetUserByEmail(string email)
        {
            string apiUrl = GetConfigurationSettings("WebApiHost") + "api/AppUser/GetUserByEmail?email=" + email;
            Models.AppUser.AppUserViewModel appUser = APIInvoker.Get<Models.AppUser.AppUserViewModel>(apiUrl);
            return appUser;
        }

        /// <summary>
        /// 加密联系信息
        /// </summary>
        /// <param name="contactInfo">[手:{resource.Mobile}],[微信:{resource.Wechat}]</param>
        /// <returns></returns>
        public static string EncryptContactInfo(string contactInfo)
        {
            if (string.IsNullOrEmpty(contactInfo)) return contactInfo;

            int p1 = contactInfo.IndexOf(":");
            if (p1 > 0) {
                string half2 = contactInfo.Substring(p1 + 1);

                if (half2.Length > 3) {
                    string beginText = half2.Substring(0, 3);
                    
                    string encryptedText = contactInfo.Substring(0, p1 + 1) + beginText + new string('*', contactInfo.Length - p1 - beginText.Length -1 - 1) + "]";
                    return encryptedText;
                }
                else {
                    string encryptedText = contactInfo.Substring(0, p1 + 1) + new string('*', contactInfo.Length - p1 - 1 - 1) + "]";
                    return encryptedText;
                }
            }
            return contactInfo;
        }

        /// <summary>
        /// 加密手机号码或座机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string EncryptPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return phone;

            if(phone.Length == 11) {
                return phone.Substring(0, 3) + "********";
            }
            else {
                if (phone.Length > 3) {
                    return phone.Substring(0, 3) + new string('*', phone.Length - 3);
                }
                else {
                    return new string('*', phone.Length);
                }
            }
        }
    }
}
