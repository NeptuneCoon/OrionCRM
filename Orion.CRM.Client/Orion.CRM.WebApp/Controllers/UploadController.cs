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
        /// �ϴ��ļ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string uploadFile()
        {
            var file = Request.Form.Files[0];
            var root = _hostingEnv.WebRootPath;
            var extension = Path.GetExtension(file.FileName);
            var guid = Guid.NewGuid().ToString();

            string filename = guid + extension;            // �µ��ļ�����
            var fullPath = $@"{root}\upload\{filename}";   // �µĴ洢·��
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
                        bool validateResult = true; // Excel��ʽ����(ֻ��֤Excel�ļ��ĵ�1��)
                        if (worksheet.Dimension == null || worksheet.Dimension.Rows <= 1) {
                            result.ErrorMsgs.Add("��Ǹ�����ϴ���Excel�ļ���û�����ݣ�");
                        }
                        else {
                            validateResult = ValidateFormat(worksheet);
                            if (!validateResult) {
                                result.ErrorMsgs.Add("Excel�ļ���ʽ������ʹ�������ṩ��ģ���ļ�¼�����ݣ�<a href=\"/download/template/��Դ��������ģ��.xlsx\">�������</a>��");
                            }
                            else {
                                // ��Excel����ȡ����
                                List<Models.Resource.Resource> resources = ExtractData(worksheet);

                                if (resources != null && resources.Count > 0) {
                                    // ��֤Excel������ȷ��
                                    string errorInfo = ValidateData(resources);

                                    if (string.IsNullOrEmpty(errorInfo)) {
                                        // ��ϴ����
                                        List<Models.Resource.ResourceEntity> datas = DataHandling(resources);

                                        string projectName = resources.FirstOrDefault().ProjectName;
                                        int projectId = _projects.FirstOrDefault(x => x.ProjectName == projectName).Id;

                                        // ���
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
                        result.ErrorMsgs.Add("��Ǹ��Excel����ʧ�ܣ�����ʹ��Microsoft Excel 2007�����ϰ汾��лл������");
                    }
                }

                return result;
            }
            catch {
                result.ErrorMsgs.Add("������һЩδ֪������ˢ��ҳ������³��Ի���ϵ����Ա��");
                return result;
            }
            finally {
                DeleteFile();
            }
        }


        #region ��Excel����ȡ����
        /// <summary>
        /// ��Excel����ȡ����
        /// </summary>
        /// <param name="worksheet">sheet����</param>
        /// <returns></returns>
        List<Models.Resource.Resource> ExtractData(ExcelWorksheet worksheet)
        {
            List<Models.Resource.Resource> resources = new List<Models.Resource.Resource>();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++) {
                Models.Resource.Resource resource = new Models.Resource.Resource();

                // �ͻ�����
                resource.CustomerName = worksheet.Cells[row, 1]?.Value?.ToString();
                // ��ϵ��ʽ(��ʱ�����Mobile�ֶΣ��������н���)
                resource.Mobile = worksheet.Cells[row, 2]?.Value?.ToString().Trim();
                // �����ϵ
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
                // ��Դ
                resource.SourceFromText = worksheet.Cells[row, 4]?.Value?.ToString();
                // �����
                resource.AppendMan = worksheet.Cells[row, 5]?.Value?.ToString();
                // ��Ŀ
                resource.ProjectName = worksheet.Cells[row, 6]?.Value?.ToString();

                resources.Add(resource);
            }

            return resources;
        }
        #endregion

        #region ��֤Excel�ļ���ʽ
        /// <summary>
        /// ��֤Excel�ļ���ʽ
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

            if (string.IsNullOrEmpty(columnTitle1) || !columnTitle1.Contains("�ͻ�����")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle2) || !columnTitle2.Contains("��ϵ��ʽ")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle3) || !columnTitle3.Contains("�����ϵ")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle4) || !columnTitle4.Contains("��Դ")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle5) || !columnTitle5.Contains("�����")) {
                validateResult = false;
            }
            if (string.IsNullOrEmpty(columnTitle6) || !columnTitle6.Contains("��Ŀ")) {
                validateResult = false;
            }

            return validateResult;
        }
        #endregion

        #region ��֤Excel���ݵ���Ч��
        /// <summary>
        /// ��֤Excel���ݵ���Ч��
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        string ValidateData(List<Models.Resource.Resource> resources)
        {
            if (resources == null || resources.Count <= 0) return "";

            StringBuilder sbError = new StringBuilder();
            for (int i = 0; i < resources.Count; i++) {
                var item = resources[i];
                // ��ֵ��֤
                if (string.IsNullOrEmpty(item.Mobile)) {
                    sbError.Append($"��{i + 2}�е�2��[��ϵ��ʽ]����Ϊ��;");
                }
                if (string.IsNullOrEmpty(item.AppendMan)) {
                    sbError.Append($"��{i + 2}�е�5��[�����]����Ϊ��;");
                }
                if (string.IsNullOrEmpty(item.ProjectName)) {
                    sbError.Append($"��{i + 2}�е�6��[��Ŀ]����Ϊ��;");
                }
                // ������Ƿ����
                if (!string.IsNullOrEmpty(item.AppendMan)) {
                    var appUser = _allUsers.FirstOrDefault(x => x.RealName == item.AppendMan || x.UserName == item.AppendMan);
                    if (appUser == null) {
                        sbError.Append($"��{i + 2}�е�5�������[{item.AppendMan}]������;");
                    }
                }
                // ��Ŀ�Ƿ����
                if (!string.IsNullOrEmpty(item.ProjectName)) {
                    var project = _projects.FirstOrDefault(x => x.ProjectName == item.ProjectName);
                    if (project == null) {
                        sbError.Append($"��{i + 2}�е�6����Ŀ[{item.ProjectName}]������;");
                    }
                }
            }
            // ��֤ͬһExcel�Ƿ�����˲�ͬ����Ŀ(1��Excel�ļ�ֻ�ܳ���һ����Ŀ������)
            List<string> projectNames = resources.Select(x => x.ProjectName).Distinct().ToList();
            if (projectNames != null && projectNames.Count > 1) {
                string projectNameStr = string.Join(",", projectNames);
                sbError.Append($"ͬһExcel�ļ��г����˶����Ŀ[{projectNameStr}]��Ϊ�˱�֤����һ���ԣ�������һ��Excel�ļ��а��������Ŀ�����ݣ�лл��");
            }

            return sbError.ToString();
        } 
        #endregion

        #region ������ϴ�ʹ���
        /// <summary>
        /// ������ϴ�ʹ���
        /// </summary>
        List<Models.Resource.ResourceEntity> DataHandling(List<Models.Resource.Resource> resources)
        {
            if (resources == null || resources.Count <= 0) return null;

            var datas = new List<Models.Resource.ResourceEntity>();
            foreach (var item in resources) {
                Models.Resource.ResourceEntity data = new Models.Resource.ResourceEntity();
                // �ͻ�����
                data.CustomerName = string.IsNullOrEmpty(item.CustomerName) ? "δ֪" : item.CustomerName;

                // ������ϵ��ʽ
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

                // �����ϵ
                data.LastTime = item.LastTime;

                // ��Դ��Դ
                data.SourceFrom = GetSourceIdByText(item.SourceFromText);
                // ��Դ״̬
                data.Status = 3;
                // �����
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
        /// ������������
        /// </summary>
        /// <param name="datas">���ݼ�</param>
        /// <param name="orgId">��֯����Id</param>
        /// <param name="projectId">��ĿId</param>
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
                            // �������ݿ�(Ϊ�˱����ظ���һ��һ������)
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
                                result.ErrorMsgs.Add("����{" + ResourceToString(data) + "}����ʧ�ܡ�");
                            }
                        }
                        else {
                            result.RepeatCount++;
                        }
                    }
                    // ��������ResourceOrganization
                    if (result.SuccessCount > 0) {
                        bool res1 = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "/api/Resource/BatchInsertResourceOrg", resourceOrgs);
                        if (res1) {
                            // ��������ResourceProject
                            bool res2 = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "/api/Resource/BatchInsertResourceProject", resourceProjects);
                            if (!res2) {
                                // �ݲ�����
                            }
                        }
                        else {
                            // ����ʧ��
                            // ɾ���ող������Դ
                            foreach (var item in resourceOrgs) {
                                DeleteResource(item.ResourceId);
                            }
                            result.SuccessCount = 0;
                            result.FailCount = result.TotalCount;
                        }
                    }
                }
                else {
                    result.ErrorMsgs.Add("δ�ܻ�ȡ��������֯��������Ŀ");
                }
            }

            return result;
        }

        #region ��Դ�Ƿ����
        bool IsResourceExist(string mobile, string tel, string qq, string wechat)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/IsResourceExist?orgId={_AppUser.OrgId}&mobile={mobile}&tel={tel}&qq={qq}&wechat={wechat}";
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }
        #endregion

        #region ������Դ InsertResource
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

        #region ɾ����Դ
        /// <summary>
        /// ɾ����Դ ajax
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
        /// ������Դ�ı���ȡ��Դ��ԴId
        /// </summary>
        /// <param name="text">��Դ��Դ�ı�</param>
        /// <returns>��ԴId</returns>
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