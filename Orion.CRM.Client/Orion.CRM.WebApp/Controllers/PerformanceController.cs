using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.Models.Sign;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 业绩统计控制器
    /// </summary>
    public class PerformanceController : BaseController
    {

        public IActionResult Month()
        {
            return View();
        }

        public IActionResult Quarter()
        {
            return View();
        }

        public IActionResult Year()
        {
            return View();
        }

        // 业绩概览
        public IActionResult Overview()
        {
            string apiBaseUrl = _AppConfig.WebApiHost + "api/CustomerSign/GetSignsByTime";

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

            return View();
        }

        // 组业绩分析
        public IActionResult Group()
        {
            return View();
        }

        /// <summary>
        /// 将 签约记录 转化为业绩视图模型
        /// </summary>
        /// <param name="signRecords"></param>
        /// <returns></returns>
        private List<Models.Sign.SignViewModel> ConvertToSignViewModel(List<CustomerSign> signRecords)
        {
            if (signRecords == null || signRecords.Count <= 0) return null;

            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(var sign in signRecords) {
                if (dict.ContainsKey(sign.GroupName)) {
                    dict[sign.GroupName] += sign.Amount;
                }
                else {
                    dict.Add(sign.GroupName, sign.Amount);
                }
            }

            List<Models.Sign.SignViewModel> list = new List<SignViewModel>();
            foreach(var item in dict) {
                list.Add(new SignViewModel() { GroupName = item.Key, GroupSaleAmount = item.Value });
            }
            return list;
        }

    }
}