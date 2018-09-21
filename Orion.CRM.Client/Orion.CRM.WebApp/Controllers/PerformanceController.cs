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
    /// 业绩统计控制器
    /// </summary>
    public class PerformanceController : BaseController
    {
        // 业绩概览
        public IActionResult Overview()
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByTime";

            // 当月业绩
            string month_beginTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 00:00:00";
            string month_endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string monthApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + month_beginTime + "&endTime=" + month_endTime;
            List<CustomerSign> monthSignRecords = APIInvoker.Get<List<CustomerSign>>(monthApiUrl);

            // 本年业绩
            string year_beginTime = DateTime.Now.Year + "-1-1 00:00:00";
            string year_endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string yearApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + year_beginTime + "&endTime=" + year_endTime;
            List<CustomerSign> yearSignRecords = APIInvoker.Get<List<CustomerSign>>(yearApiUrl);

            ViewBag.MonthSigns = ConvertToSignViewModel(monthSignRecords);
            ViewBag.YearSigns = ConvertToSignViewModel(yearSignRecords);

            // 以下数据从当朋数据monthSignRecords中获取
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

            // 注意：最具潜力业务排行的数据来源是当月数据(默认从当月1号开始统计)
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

        // 组业绩分析
        public IActionResult Group()
        {
            Models.Performance.GroupSaleViewModel viewModel = new Models.Performance.GroupSaleViewModel();

            // 获取本月组成员业绩统计
            viewModel.StartDateMember = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1";
            viewModel.EndDateMember = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            // 组业绩变化趋势图的默认时间范围
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
            
            // 获取默认的第一个项目下各组的业绩变化趋势
            
            // step1.获取ProjectId
            int projectId = 0;
            if(_AppUser.ProjectId == null) {
                if(viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) { 
                    projectId = viewModel.ProjectList[0].Id;
                }
            }
            else {
                projectId = Convert.ToInt32(_AppUser.ProjectId);
            }
            // step2.获取该Project下每个组的业绩
            ViewBag.ProjectGroupSignRecords = GetProjectGroupSignRecords(projectId, viewModel.StartDateTrend, viewModel.EndDateTrend);

            // step3.获取某段时间内组业绩
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
                viewModel.StartDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1";//默认日期为本月1号至当天
            }

            if (!string.IsNullOrEmpty(endDate)) {
                viewModel.EndDate = endDate;
            }
            else { 
                viewModel.EndDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;//默认为当天
            }

            viewModel.ProjectList = App_Data.AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            if (viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) {
                if (projectId == 0) { 
                    viewModel.GroupList = App_Data.AppDTO.GetGroupsByProjectId(viewModel.ProjectList[0].Id);
                }
                else {
                    viewModel.GroupList = App_Data.AppDTO.GetGroupsByProjectId(projectId);
                }
                if (projectId == 0) {//第一次加载
                    projectId = viewModel.ProjectList[0].Id;//默认为第一个项目
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

            // 部分参数初始化
            if (param.pi <= 0) param.pi = 1;
            param.oid = _AppUser.OrgId;
            param.ps = _AppConfig.PageSize;

            // 查询数据
            var list = APIInvoker.Post<List<Models.Sign.CustomerSign>>(_AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByCondition", param);
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/CustomerSign/GetSignCountByCondition", param);

            //分页参数
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
            
            // 默认项目
            int? projectId = _AppUser.ProjectId;
            var projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            if (projectId == null || projectId <= 0) {
                projectId = projects?.FirstOrDefault()?.Id;//列表中的第一个
            }
            ViewBag.DefaultProjectId = Convert.ToInt32(projectId);

            // 组织机构下的业务员
            string apiUser = _AppConfig.WebApiHost + "/api/AppUser/GetUsersByOrgId?pageIndex=1&pageSize=2000&orgId=" + _AppUser.OrgId;
            viewModel.OrgUsers = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUser);

            // 项目列表
            viewModel.ProjectList = projects;

            return View(viewModel);
        }

        #region 指定时间段内的业务员业绩排行榜
        [HttpGet]
        public List<Models.Performance.SignRank> GetMemberRanking(string startDate, string endDate)
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignsByTime";
            // 当月业绩
            string month_beginTime = startDate + " 00:00:00";
            string month_endTime = endDate + " 23:59:59";
            string monthApiUrl = apiBaseUrl + "?orgId=" + _AppUser.OrgId + "&beginTime=" + month_beginTime + "&endTime=" + month_endTime;
            List<CustomerSign> monthSignRecords = APIInvoker.Get<List<CustomerSign>>(monthApiUrl);

            // 以下数据从当朋数据monthSignRecords中获取
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

            // 注意：最具潜力业务排行的数据来源是当月数据(默认从当月1号开始统计)
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

        #region 获取组下每个成员在某时间区间内的签约记录
        // 获取组下每个成员在某时间区间内的签约记录
        public List<Models.Performance.SignRank> GetGroupMemberSignRecords(int groupId, string startDate, string endDate)
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetGroupMemberSigns";
            if (groupId > 0) {
                // 获取默认的第一个组的成员业绩
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

        #region 获取项目下每个组业绩变化趋势
        public string GetProjectGroupSignRecords(int projectId, string startDate, string endDate)
        {
            if (projectId <= 0) return "";

            // 取最近半年的数据，进行变化趋势的展示
            //string startDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            //string endDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (projectId > 0) {
                // 获取默认的第一个组的成员业绩
                string apiBaseUrl = _AppConfig.WebApiHost + "/api/CustomerSign/GetProjectGroupSigns";
                string apiUrl = apiBaseUrl + "?projectId=" + projectId + "&beginTime=" + startDate + "&endTime=" + endDate;
                List<CustomerSign> signRecords = APIInvoker.Get<List<CustomerSign>>(apiUrl);
                if (signRecords != null && signRecords.Count > 0) {
                    signRecords.Where(x => x.GroupName == null).ToList().ForEach(x => x.GroupName = "未知");//有些签约业务可能不属于业务组(经理，总经理等)
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

        #region 获取某段时间内组业绩 + GetGroupSaleRanking
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

        #region 将 签约记录 转化为业绩视图模型
        /// <summary>
        /// 将 签约记录 转化为业绩视图模型
        /// </summary>
        /// <param name="signRecords"></param>
        /// <returns></returns>
        private List<Models.Sign.SignViewModel> ConvertToSignViewModel(List<CustomerSign> signRecords)
        {
            if (signRecords == null || signRecords.Count <= 0) return null;

            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var sign in signRecords) {
                if (string.IsNullOrEmpty(sign.GroupName)) sign.GroupName = "未知";
                if (dict.ContainsKey(sign.GroupName)) {
                    dict[sign.GroupName] += sign.Amount;
                }
                else {
                    dict.Add(sign.GroupName, sign.Amount);
                }
            }

            // 排序
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
            int count = APIInvoker.Get<int>(apiUrl);//受影响的行数

            TempData["result"] = count > 0;

            return count;
        }
    }
}