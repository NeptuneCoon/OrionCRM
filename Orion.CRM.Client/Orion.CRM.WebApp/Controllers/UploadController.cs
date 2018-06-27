using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnv;
        private List<Models.Source.Source> _sources;
        private List<Models.AppUser.AppUserComplex> _allUsers;
        private List<Models.Project.Project> _projects;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string uploadFile()
        {
            var file = Request.Form.Files[0];
            var root = _hostingEnv.WebRootPath;
            var extension = Path.GetExtension(file.FileName);
            var guid = Guid.NewGuid().ToString();

            string filename = guid + extension;            // 新的文件名称
            var fullPath = $@"{root}\upload\{filename}";   // 新的存储路径
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                file.CopyToAsync(stream);
            }
            return filename;
        }

        [HttpGet]
        public Models.Upload.InsertResult ImportData(string filename)
        {
            _sources = App_Data.AppDTO.GetSourcesFromDb(_AppUser.OrgId);
            _allUsers = GetAllUsers(_AppUser.OrgId);
            _projects = App_Data.AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            Models.Upload.InsertResult result = new Models.Upload.InsertResult();

            var filepath = $@"{_hostingEnv.WebRootPath}\upload\{filename}";
            FileInfo file = new FileInfo(filepath);
            try {
                using (ExcelPackage package = new ExcelPackage(file)) {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    if (worksheet != null) {                   
                        bool validateResult = true; // Excel格式检验(只验证Excel文件的第1行)
                        if (worksheet.Dimension == null || worksheet.Dimension.Rows <= 1) {
                            result.ErrorMsgs.Add("抱歉，您上传的Excel文件中没有数据！");
                        }
                        else {
                            validateResult = ValidateFormat(worksheet);
                            if (!validateResult) {
                                result.ErrorMsgs.Add("Excel文件格式有误！请使用我们提供的模板文件录入数据，<a href=\"/download/template/资源批量导入模板.xlsx\">点击下载</a>。");
                            }
                            else {
                                // 从Excel中提取数据
                                List<Models.Resource.Resource> resources = ExtractData(worksheet);

                                if (resources != null && resources.Count > 0) {
                                    // 验证Excel数据正确性
                                    string errorInfo = ValidateData(resources);

                                    if (string.IsNullOrEmpty(errorInfo)) {
                                        // 清洗数据
                                        List<Models.Resource.ResourceEntity> datas = DataHandling(resources);

                                        string projectName = resources.FirstOrDefault().ProjectName;
                                        int projectId = _projects.FirstOrDefault(x => x.ProjectName == projectName).Id;

                                        // 入库
                                        Models.Upload.InsertResult insertResult = DataBatchInsert(datas, _AppUser.OrgId, projectId);
                                        result.SuccessCount = insertResult.SuccessCount;
                                        result.TotalCount = insertResult.TotalCount;
                                        result.RepeatCount = insertResult.RepeatCount;
                                        result.FailCount = insertResult.FailCount;
                                        if (insertResult.ErrorMsgs != null && insertResult.ErrorMsgs.Count > 0) {
                                            result.ErrorMsgs.AddRange(insertResult.ErrorMsgs);
                                        }

                                        return result;
                                    }
                                    else {
                                        result.ErrorMsgs.Add(errorInfo);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        result.ErrorMsgs.Add("抱歉，Excel解析失败！请您使用Microsoft Excel 2007及以上版本，谢谢合作！");
                    }
                }

                return result;
            }
            catch {
                result.ErrorMsgs.Add("发生了一些未知错误，请刷新页面后重新尝试或联系管理员。");
                return result;
            }
            finally {
                DeleteFile();
            }
        }


        #region 从Excel中提取数据
        /// <summary>
        /// 从Excel中提取数据
        /// </summary>
        /// <param name="worksheet">sheet对象</param>
        /// <returns></returns>
        List<Models.Resource.Resource> ExtractData(ExcelWorksheet worksheet)
        {
            List<Models.Resource.Resource> resources = new List<Models.Resource.Resource>();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++) {
                Models.Resource.Resource resource = new Models.Resource.Resource();

                // 客户姓名
                resource.CustomerName = worksheet.Cells[row, 1]?.Value?.ToString();
                // 联系方式(暂时存放在Mobile字段，后面会进行解析)
                resource.Mobile = worksheet.Cells[row, 2]?.Value?.ToString().Trim();
                // 最后联系
                string lastTime = worksheet.Cells[row, 3]?.Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(lastTime)) {
                    DateTime dt = DateTime.Now;
                    if (DateTime.TryParse(lastTime, out dt)) {
                        resource.LastTime = dt;
                    }
                    else {
                        resource.LastTime = DateTime.Now;
                    }
                }
                // 来源
                resource.SourceFromText = worksheet.Cells[row, 4]?.Value?.ToString();
                // 添加人
                resource.AppendMan = worksheet.Cells[row, 5]?.Value?.ToString();
                // 项目
                resource.ProjectName = worksheet.Cells[row, 6]?.Value?.ToString();

                resources.Add(resource);
            }

            return resources;
        }
        #endregion

        #region 验证Excel文件格式
        /// <summary>
        /// 验证Excel文件格式
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        bool ValidateFormat(ExcelWorksheet worksheet)
        {
            bool validateResult = true;
            string columnTitle1 = worksheet.Cells[1, 1]?.Value?.ToString();
            string columnTitle2 = worksheet.Cells[1, 2]?.Value?.ToString();
            string columnTitle3 = worksheet.Cells[1, 3]?.Value?.ToString();
            string columnTitle4 = worksheet.Cells[1, 4]?.Value?.ToString();
            string columnTitle5 = worksheet.Cells[1, 5]?.Value?.ToString();
            string columnTitle6 = worksheet.Cells[1, 6]?.Value?.ToString();

            if (string.IsNullOrEmpty(columnTitle1) || !columnTitle1.Contains("客户姓名")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle2) || !columnTitle2.Contains("联系方式")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle3) || !columnTitle3.Contains("最后联系")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle4) || !columnTitle4.Contains("来源")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle5) || !columnTitle5.Contains("添加人")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle6) || !columnTitle6.Contains("项目")) {
                validateResult = false;
            }

            return validateResult;
        }
        #endregion

        #region 验证Excel数据的有效性
        /// <summary>
        /// 验证Excel数据的有效性
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        string ValidateData(List<Models.Resource.Resource> resources)
        {
            if (resources == null || resources.Count <= 0) return "";

            StringBuilder sbError = new StringBuilder();
            for (int i = 0; i < resources.Count; i++) {
                var item = resources[i];
                // 空值验证
                if (string.IsNullOrEmpty(item.Mobile)) {
                    sbError.Append($"第{i + 2}行第2列[联系方式]不能为空;");
                }
                if (string.IsNullOrEmpty(item.AppendMan)) {
                    sbError.Append($"第{i + 2}行第5列[添加人]不能为空;");
                }
                if (string.IsNullOrEmpty(item.ProjectName)) {
                    sbError.Append($"第{i + 2}行第6列[项目]不能为空;");
                }
                // 添加人是否存在
                if (!string.IsNullOrEmpty(item.AppendMan)) {
                    var appUser = _allUsers.FirstOrDefault(x => x.RealName == item.AppendMan || x.UserName == item.AppendMan);
                    if (appUser == null) {
                        sbError.Append($"第{i + 2}行第5列添加人[{item.AppendMan}]不存在;");
                    }
                }
                // 项目是否存在
                if (!string.IsNullOrEmpty(item.ProjectName)) {
                    var project = _projects.FirstOrDefault(x => x.ProjectName == item.ProjectName);
                    if (project == null) {
                        sbError.Append($"第{i + 2}行第6列项目[{item.ProjectName}]不存在;");
                    }
                }
            }
            // 验证同一Excel是否出现了不同的项目(1个Excel文件只能出现一个项目的数据)
            List<string> projectNames = resources.Select(x => x.ProjectName).Distinct().ToList();
            if (projectNames != null && projectNames.Count > 1) {
                string projectNameStr = string.Join(",", projectNames);
                sbError.Append($"同一Excel文件中出现了多个项目[{projectNameStr}]！为了保证数据一致性，请勿在一个Excel文件中包含多个项目的数据，谢谢！");
            }

            return sbError.ToString();
        } 
        #endregion

        #region 数据清洗和处理
        /// <summary>
        /// 数据清洗和处理
        /// </summary>
        List<Models.Resource.ResourceEntity> DataHandling(List<Models.Resource.Resource> resources)
        {
            if (resources == null || resources.Count <= 0) return null;

            var datas = new List<Models.Resource.ResourceEntity>();
            foreach (var item in resources) {
                Models.Resource.ResourceEntity data = new Models.Resource.ResourceEntity();
                // 客户姓名
                data.CustomerName = string.IsNullOrEmpty(item.CustomerName) ? "未知" : item.CustomerName;

                // 解析联系方式
                if (!string.IsNullOrEmpty(item.Mobile)) {
                    long n = 0;
                    if (long.TryParse(item.Mobile, out n)) {
                        if (item.Mobile.Length == 11) {
                            data.Mobile = item.Mobile;
                        }
                        else {
                            data.QQ = item.Mobile;
                        }
                    }
                    else {
                        data.Wechat = item.Mobile;
                    }
                }

                // 最后联系
                data.LastTime = item.LastTime;

                // 资源来源
                data.SourceFrom = GetSourceIdByText(item.SourceFromText);
                // 资源状态
                data.Status = 3;
                // 添加人
                if (!string.IsNullOrEmpty(item.AppendMan)) {
                    data.AppendUserId = _allUsers.FirstOrDefault(x => x.RealName == item.AppendMan || x.UserName == item.AppendMan)?.Id;
                }

                data.CreateTime = DateTime.Now;
                data.UpdateTime = DateTime.Now;

                datas.Add(data);
            }

            return datas;
        }
        #endregion

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="datas">数据集</param>
        /// <param name="orgId">组织机构Id</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        Models.Upload.InsertResult DataBatchInsert(List<Models.Resource.ResourceEntity> datas,int orgId, int projectId)
        {
            Models.Upload.InsertResult result = new Models.Upload.InsertResult();

            if (datas != null && datas.Count > 0) {
                result.TotalCount = datas.Count;
                List<Models.Resource.ResourceOrganization> resourceOrgs = new List<Models.Resource.ResourceOrganization>();
                List<Models.Resource.ResourceProject> resourceProjects = new List<Models.Resource.ResourceProject>();

                if (orgId > 0 && projectId > 0) {
                    foreach (var data in datas) {
                        bool isExist = IsResourceExist(data.Mobile, data.Tel, data.QQ, data.Wechat);
                        if (!isExist) {
                            // 插入数据库(为了避免重复，一条一条插入)
                            int id = InsertResource(data);
                            if (id > 0) {
                                result.SuccessCount++;
                                resourceOrgs.Add(new Models.Resource.ResourceOrganization() {
                                    OrgId = orgId,
                                    ResourceId = id,
                                    CreateTime = DateTime.Now
                                });
                                resourceProjects.Add(new Models.Resource.ResourceProject() {
                                    ProjectId = projectId,
                                    ResourceId = id,
                                    CreateTime = DateTime.Now
                                });
                            }
                            else {
                                result.FailCount++;
                                result.ErrorMsgs.Add("数据{" + ResourceToString(data) + "}插入失败。");
                            }
                        }
                        else {
                            result.RepeatCount++;
                        }
                    }
                    // 批量插入ResourceOrganization
                    if (result.SuccessCount > 0) {
                        bool res1 = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "/api/Resource/BatchInsertResourceOrg", resourceOrgs);
                        if (res1) {
                            // 批量插入ResourceProject
                            bool res2 = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "/api/Resource/BatchInsertResourceProject", resourceProjects);
                            if (!res2) {
                                // 暂不处理
                            }
                        }
                        else {
                            // 插入失败
                            // 删除刚刚插入的资源
                            foreach (var item in resourceOrgs) {
                                DeleteResource(item.ResourceId);
                            }
                            result.SuccessCount = 0;
                            result.FailCount = result.TotalCount;
                        }
                    }
                }
                else {
                    result.ErrorMsgs.Add("未能获取到所属组织或所属项目");
                }
            }

            return result;
        }

        #region 资源是否存在
        bool IsResourceExist(string mobile, string tel, string qq, string wechat)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/IsResourceExist?orgId={_AppUser.OrgId}&mobile={mobile}&tel={tel}&qq={qq}&wechat={wechat}";
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }
        #endregion

        #region 插入资源 InsertResource
        int InsertResource(Models.Resource.ResourceEntity data)
        {
            var resource = new
            {
                CustomerName = data.CustomerName?.Trim(),
                Message = data.Message?.Trim(),
                Mobile = data.Mobile?.Trim(),
                Tel = data.Tel?.Trim(),
                Wechat = data.Wechat?.Trim(),
                QQ = data.QQ?.Trim(),
                Email = data.Email,
                Address = data.Address,
                LastTime = data.LastTime,
                Inclination = data.Inclination,
                TalkCount = 0,
                SourceFrom = data.SourceFrom,
                Status = data.Status,
                AppendUserId = _AppUser.Id,
                Remark = data.Remark?.Trim(),
                CreateTime = data.CreateTime,
                UpdateTime = data.CreateTime,
                DeleteFlag = 0
            };

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResource";
            int identityId = APIInvoker.Post<int>(apiUrl, resource);

            return identityId;
        }
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除资源 ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteResource(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/DeleteResource?id=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }
        #endregion

        string ResourceToString(Models.Resource.ResourceEntity resource)
        {
            string str = "";
            
            str += resource.CustomerName+",";
            str += resource.Mobile + resource.Wechat + resource.QQ + ",";
            str += resource.LastTime?.ToString("yyyy-MM-dd HH:mm:ss") + ",";

            if (resource.AppendUserId != null && resource.AppendUserId > 0) {
                var appUser = _allUsers.FirstOrDefault(x => x.Id == resource.AppendUserId);
                if (appUser != null) {
                    str += appUser.RealName;
                }
            }

            return str;
        }

        /// <summary>
        /// 根据来源文本获取资源来源Id
        /// </summary>
        /// <param name="text">资源来源文本</param>
        /// <returns>来源Id</returns>
        int? GetSourceIdByText(string text)
        {
            return _sources.FirstOrDefault(x => x.SourceName == text.Trim())?.Id;
        }

        List<Models.AppUser.AppUserComplex> GetAllUsers(int orgId)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/AppUser/GetUsersByOrgId?pageIndex=1&pageSize=10000&orgId=" + _AppUser.OrgId;
            List<Models.AppUser.AppUserComplex> users = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUrl);
            return users;
        }

        void DeleteFile()
        {
            try {
                string path = $@"{_hostingEnv.WebRootPath}\upload\";
                string[] files = System.IO.Directory.GetFiles(path);
                foreach(var file in files) {
                    System.IO.File.Delete(file);
                }
            }
            catch {
                // ...
            }
        }
    }
}