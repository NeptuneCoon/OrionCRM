using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.Models.Sign;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ҵ��ͳ�ƿ�����
    /// </summary>
    public class PerformanceController : BaseController
    {
        // ҵ������
        public IActionResult Overview()
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByTime";

            // ����ҵ��
            string month_beginTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 00:00:00";
            string month_endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string monthApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + month_beginTime + "&endTime=" + month_endTime;
            List<CustomerSign> monthSignRecords = APIInvoker.Get<List<CustomerSign>>(monthApiUrl);

            // ����ҵ��
            string year_beginTime = DateTime.Now.Year + "-1-1 00:00:00";
            string year_endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string yearApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + year_beginTime + "&endTime=" + year_endTime;
            List<CustomerSign> yearSignRecords = APIInvoker.Get<List<CustomerSign>>(yearApiUrl);

            ViewBag.MonthSigns = ConvertToSignViewModel(monthSignRecords);
            ViewBag.YearSigns = ConvertToSignViewModel(yearSignRecords);

            // �������ݴӵ�������monthSignRecords�л�ȡ
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if(monthSignRecords != null && monthSignRecords.Count > 0) {
                foreach(var record in monthSignRecords) {
                    if(dict.ContainsKey(record.SignMan)){
                        dict[record.SignMan] += record.Amount;
                    }
                    else {
                        dict.Add(record.SignMan, record.Amount);
                    }
                }
            }
            dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            dict = dict.Take(20).ToDictionary(x => x.Key, x => x.Value);

            // ע�⣺���Ǳ��ҵ�����е�������Դ�ǵ�������(Ĭ�ϴӵ���1�ſ�ʼͳ��)
            if(dict != null && dict.Keys.Count > 0) {
                List<Models.Performance.SignRank> rankRecords = new List<Models.Performance.SignRank>();
                decimal totalAmount = dict.Sum(x => x.Value);
                foreach(var item in dict) {
                    Models.Performance.SignRank rank = new Models.Performance.SignRank();
                    rank.SignMan = item.Key;
                    rank.Amount = item.Value;
                    if (totalAmount != 0) {
                        rank.Percent = (item.Value / totalAmount * 100).ToString("f1");
                    }
                    else {
                        rank.Percent = "0.0";
                    }

                    rankRecords.Add(rank);
                }

                ViewBag.RankRecords = rankRecords;
            }
            
            return View();
        }

        // ��ҵ������
        public IActionResult Group()
        {
            Models.Performance.GroupSaleViewModel viewModel = new Models.Performance.GroupSaleViewModel();

            // ��ȡ�������Աҵ��ͳ��
            viewModel.StartDateMember = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1";
            viewModel.EndDateMember = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            // ��ҵ���仯����ͼ��Ĭ��ʱ�䷶Χ
            viewModel.StartDateTrend = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            viewModel.EndDateTrend = DateTime.Now.ToString("yyyy-MM-dd");

            int firstGroupId = 0;
            viewModel.ProjectList = App_Data.AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            if (viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) {
                viewModel.GroupList = App_Data.AppDTO.GetGroupsByProjectId(viewModel.ProjectList[0].Id);
                if(viewModel.GroupList != null && viewModel.GroupList.Count > 0) {
                    firstGroupId = viewModel.GroupList.First().Id;
                }
            }

            ViewBag.MemberSignRecords = GetGroupMemberSignRecords(firstGroupId, viewModel.StartDateMember + " 00:00:00", viewModel.EndDateMember + " 23:59:59");
            
            // ��ȡĬ�ϵĵ�һ����Ŀ�¸����ҵ���仯����
            
            // step1.��ȡProjectId
            int projectId = 0;
            if(_AppUser.ProjectId == null) {
                if(viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) { 
                    projectId = viewModel.ProjectList[0].Id;
                }
            }
            else {
                projectId = Convert.ToInt32(_AppUser.ProjectId);
            }
            // step2.��ȡ��Project��ÿ�����ҵ��
            ViewBag.ProjectGroupSignRecords = GetProjectGroupSignRecords(projectId, viewModel.StartDateTrend, viewModel.EndDateTrend);

            // step3.��ȡĳ��ʱ������ҵ��
            ViewBag.GroupSaleRanking = GetGroupSaleRanking(projectId, viewModel.StartDateMember, viewModel.EndDateMember);

            return View(viewModel);
        }

        public IActionResult Talkcount(int projectId, int groupId, string startDate, string endDate)
        {
            Models.Performance.TalkRecordStatViewModel viewModel = new Models.Performance.TalkRecordStatViewModel();

            if (!string.IsNullOrEmpty(startDate)) {
                viewModel.StartDate = startDate;
            }
            else {
                viewModel.StartDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1";//Ĭ������Ϊ����1��������
            }

            if (!string.IsNullOrEmpty(endDate)) {
                viewModel.EndDate = endDate;
            }
            else { 
                viewModel.EndDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;//Ĭ��Ϊ����
            }

            viewModel.ProjectList = App_Data.AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            if (viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) {
                if (projectId == 0) { 
                    viewModel.GroupList = App_Data.AppDTO.GetGroupsByProjectId(viewModel.ProjectList[0].Id);
                }
                else {
                    viewModel.GroupList = App_Data.AppDTO.GetGroupsByProjectId(projectId);
                }
                if (projectId == 0) {//��һ�μ���
                    projectId = viewModel.ProjectList[0].Id;//Ĭ��Ϊ��һ����Ŀ
                }
            }

            string urlParams = $"?orgId={_AppUser.OrgId}&projectId={projectId}&groupId={groupId}&beginTime={viewModel.StartDate + " 00:00:00"}&endTime={viewModel.EndDate + " 23:59:59"}";
            string apiUrl = _AppConfig.WebApiHost + "/api/TalkRecord/TalkRecordStat" + urlParams;
            var talkcountRanks = APIInvoker.Get<List<Models.Performance.TalkcountRank>>(apiUrl);
            viewModel.TalkcountRanks = talkcountRanks;
            if (viewModel.TalkcountRanks != null) {
               viewModel.TalkcountRanks = viewModel.TalkcountRanks.OrderByDescending(x => x.Count).ToList();
            }

            return View(viewModel);
        }

        public IActionResult Sign(SignSearchParams param)
        {
            SignListViewModel viewModel = new SignListViewModel();

            // ���ֲ�����ʼ��
            if (param.pi <= 0) param.pi = 1;
            param.oid = _AppUser.OrgId;
            param.ps = _AppConfig.PageSize;

            // ��ѯ����
            var list = APIInvoker.Post<List<Models.Sign.CustomerSign>>(_AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByCondition", param);
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/CustomerSign/GetSignCountByCondition", param);

            //��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = totalCount,
                RouteUrl = "/Performance/Sign",
                QueryString = Request.QueryString.Value
            };
            ViewBag.PagerOption = pageOption;

            viewModel.Params = param;
            viewModel.Signs = list;
            
            // Ĭ����Ŀ
            int? projectId = _AppUser.ProjectId;
            var projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            if (projectId == null || projectId <= 0) {
                projectId = projects?.FirstOrDefault()?.Id;//�б��еĵ�һ��
            }
            ViewBag.DefaultProjectId = Convert.ToInt32(projectId);

            // ��֯�����µ�ҵ��Ա
            string apiUser = _AppConfig.WebApiHost + "/api/AppUser/GetUsersByOrgId?pageIndex=1&pageSize=2000&orgId=" + _AppUser.OrgId;
            viewModel.OrgUsers = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUser);

            // ��Ŀ�б�
            viewModel.ProjectList = projects;

            return View(viewModel);
        }

        #region ָ��ʱ����ڵ�ҵ��Աҵ�����а�
        [HttpGet]
        public List<Models.Performance.SignRank> GetMemberRanking(string startDate, string endDate)
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByTime";
            // ����ҵ��
            string month_beginTime = startDate + " 00:00:00";
            string month_endTime = endDate + " 23:59:59";
            string monthApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + month_beginTime + "&endTime=" + month_endTime;
            List<CustomerSign> monthSignRecords = APIInvoker.Get<List<CustomerSign>>(monthApiUrl);

            // �������ݴӵ�������monthSignRecords�л�ȡ
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if (monthSignRecords != null && monthSignRecords.Count > 0) {
                foreach (var record in monthSignRecords) {
                    if (dict.ContainsKey(record.SignMan)) {
                        dict[record.SignMan] += record.Amount;
                    }
                    else {
                        dict.Add(record.SignMan, record.Amount);
                    }
                }
            }
            dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            dict = dict.Take(20).ToDictionary(x => x.Key, x => x.Value);

            // ע�⣺���Ǳ��ҵ�����е�������Դ�ǵ�������(Ĭ�ϴӵ���1�ſ�ʼͳ��)
            List<Models.Performance.SignRank> rankRecords = new List<Models.Performance.SignRank>();
            if (dict != null && dict.Keys.Count > 0) {
                decimal totalAmount = dict.Sum(x => x.Value);
                foreach (var item in dict) {
                    Models.Performance.SignRank rank = new Models.Performance.SignRank();
                    rank.SignMan = item.Key;
                    rank.Amount = item.Value;
                    if (totalAmount != 0) {
                        rank.Percent = (item.Value / totalAmount * 100).ToString("f1");
                    }
                    else {
                        rank.Percent = "0.0";
                    }
                    rankRecords.Add(rank);
                }
            }
            return rankRecords;
        }
        #endregion

        #region ��ȡ����ÿ����Ա��ĳʱ�������ڵ�ǩԼ��¼
        // ��ȡ����ÿ����Ա��ĳʱ�������ڵ�ǩԼ��¼
        public List<Models.Performance.SignRank> GetGroupMemberSignRecords(int groupId, string startDate, string endDate)
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetGroupMemberSigns";
            if (groupId > 0) {
                // ��ȡĬ�ϵĵ�һ����ĳ�Աҵ��
                string memberApiUrl = apiBaseUrl + "?groupId=" + groupId + "&beginTime=" + startDate + "&endTime=" + endDate;
                List<CustomerSign> memberSignRecords = APIInvoker.Get<List<CustomerSign>>(memberApiUrl);
                if (memberSignRecords != null && memberSignRecords.Count > 0) {

                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    if (memberSignRecords != null && memberSignRecords.Count > 0) {
                        foreach (var record in memberSignRecords) {
                            if (dict.ContainsKey(record.SignMan)) {
                                dict[record.SignMan] += record.Amount;
                            }
                            else {
                                dict.Add(record.SignMan, record.Amount);
                            }
                        }
                    }
                    dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    if (dict != null && dict.Keys.Count > 0) {
                        List<Models.Performance.SignRank> rankRecords = new List<Models.Performance.SignRank>();
                        foreach (var item in dict) {
                            Models.Performance.SignRank rank = new Models.Performance.SignRank();
                            rank.SignMan = item.Key;
                            rank.Amount = item.Value;

                            rankRecords.Add(rank);
                        }

                        return rankRecords;
                    }
                }
            }
            return null;
        }
        #endregion

        #region ��ȡ��Ŀ��ÿ����ҵ���仯����
        public string GetProjectGroupSignRecords(int projectId, string startDate, string endDate)
        {
            if (projectId <= 0) return "";

            // ȡ�����������ݣ����б仯���Ƶ�չʾ
            //string startDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            //string endDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (projectId > 0) {
                // ��ȡĬ�ϵĵ�һ����ĳ�Աҵ��
                string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetProjectGroupSigns";
                string apiUrl = apiBaseUrl + "?projectId=" + projectId + "&beginTime=" + startDate + "&endTime=" + endDate;
                List<CustomerSign> signRecords = APIInvoker.Get<List<CustomerSign>>(apiUrl);
                if (signRecords != null && signRecords.Count > 0) {
                    signRecords.Where(x => x.GroupName == null).ToList().ForEach(x => x.GroupName = "δ֪");//��ЩǩԼҵ����ܲ�����ҵ����(�����ܾ����)
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("{\"data\":[");

                    DateTime start = Convert.ToDateTime(startDate);

                    List<string> groupNames = signRecords.Select(x => x.GroupName).Distinct().ToList();
                    while (start < Convert.ToDateTime(endDate)) {
                        DateTime end = start.AddMonths(1);
                        sb.Append("{ \"period\": \"" + start.Year + "-" + start.Month + "\", ");
                        for (int i = 0; i < groupNames.Count; i++) {
                            string groupName = groupNames[i];
                            var records = GetRecordsByRegion(signRecords, start, end, groupName);
                            int groupTotal = records.Sum(x => x.Amount);

                            if (i != groupNames.Count - 1) {
                                sb.Append($"\"{groupName.Trim()}\": {groupTotal},");
                            }
                            else {
                                sb.Append($"\"{groupName.Trim()}\": {groupTotal}");
                            }
                        }
                        start = end;
                        sb.Append("},");
                    }
                    if (sb.Length > 7) {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    string groupArrText = GetGroupNameArrText(groupNames);
                    sb.Append("],\"groups\":\"" + groupArrText + "\"}");

                    string jsonData = sb.ToString();
                    return jsonData;
                }
            }
            return "";
        }
        #endregion

        #region ��ȡĳ��ʱ������ҵ�� + GetGroupSaleRanking
        [HttpGet]
        public List<Models.Performance.GroupSaleRanking> GetGroupSaleRanking(int projectId, string startDate, string endDate)
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetGroupSaleRanking";
            string apiUrl = apiBaseUrl + $"?orgId={_AppUser.OrgId}&projectId={projectId}&beginTime={startDate + " 00:00:00"}&endTime={endDate + " 23:59:59"}";
            List<Models.Performance.GroupSaleRanking> rankings = APIInvoker.Get<List<Models.Performance.GroupSaleRanking>>(apiUrl);

            return rankings;
        } 
        #endregion

        private List<CustomerSign> GetRecordsByRegion(List<CustomerSign> records,DateTime startDate, DateTime endDate,string groupName)
        {
            if (records == null || records.Count <= 0) return null;
            List<CustomerSign> filtered = new List<CustomerSign>();

            return records.Where(x => x.SignTime >= startDate && x.SignTime < endDate && x.GroupName == groupName).ToList();
        }

        private string GetGroupNameArrText(List<string> groupNames)
        {
            string groupArrText = "";
            if (groupNames != null && groupNames.Count > 0) {
                for (int i = 0; i < groupNames.Count; i++) {
                    if (i != groupNames.Count - 1) {
                        groupArrText += "" + groupNames[i].Trim() + ",";
                    }
                    else {
                        groupArrText += "" + groupNames[i].Trim() + "";
                    }
                }
            }
            return groupArrText;
        }

        #region �� ǩԼ��¼ ת��Ϊҵ����ͼģ��
        /// <summary>
        /// �� ǩԼ��¼ ת��Ϊҵ����ͼģ��
        /// </summary>
        /// <param name="signRecords"></param>
        /// <returns></returns>
        private List<Models.Sign.SignViewModel> ConvertToSignViewModel(List<CustomerSign> signRecords)
        {
            if (signRecords == null || signRecords.Count <= 0) return null;

            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var sign in signRecords) {
                if (string.IsNullOrEmpty(sign.GroupName)) sign.GroupName = "δ֪";
                if (dict.ContainsKey(sign.GroupName)) {
                    dict[sign.GroupName] += sign.Amount;
                }
                else {
                    dict.Add(sign.GroupName, sign.Amount);
                }
            }

            // ����
            dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            List<Models.Sign.SignViewModel> list = new List<SignViewModel>();
            foreach (var item in dict) {
                list.Add(new SignViewModel() { GroupName = item.Key, GroupSaleAmount = item.Value });
            }
            return list;
        } 
        #endregion

        public int DeleteSign(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/CustomerSign/Delete?id=" + id;
            int count = APIInvoker.Get<int>(apiUrl);//��Ӱ�������

            TempData["result"] = count > 0;

            return count;
        }
    }
}