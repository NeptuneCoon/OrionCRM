using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class ReminderController : BaseController
    {
        // ajax(Resource/Detail调用)
        [HttpPost]
        public bool InsertReminder(Models.Reminder.MessageReminder reminder)
        {
            reminder.Closed = 0;
            reminder.Type = 0;
            reminder.UserId = _AppUser.Id;
            reminder.CreateTime = DateTime.Now;
            reminder.UpdateTime = DateTime.Now;

            // 插入数据库
            string apiUrl = _AppConfig.WebApiHost + "/api/MessageReminder/InsertReminder";
            bool result = APIInvoker.Post<bool>(apiUrl, reminder);

            return result;
        }

        // ajax(Resource/Detail调用)
        [HttpGet]
        public bool CloseReminder(int id)
        {
            // 更新MessageReminder
            string apiUrl = _AppConfig.WebApiHost + "/api/MessageReminder/CloseReminder?id=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);

            return result;
        }

        // ajax(Resource/Detail调用)
        [HttpGet]
        public bool DeleteReminder(int id)
        {
            // 更新MessageReminder
            string apiUrl = _AppConfig.WebApiHost + "/api/MessageReminder/DeleteReminder?id=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);

            return result;
        }

        /// <summary>
        /// 提醒
        /// </summary>
        /// <param name="begin">提醒时间-开始</param>
        /// <param name="end">提醒时间-截止</param>
        /// <param name="i">页面内部nav的索引，从0开始</param>
        /// <returns></returns>
        public IActionResult List(string begin, string end, int? i)
        {
            int userId = _AppUser.Id;
            // 默认为今日提醒
            if (string.IsNullOrEmpty(begin)) begin = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            if (string.IsNullOrEmpty(end)) end = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            if (i == null || i <= 0) i = 2;

            string apiUrl = _AppConfig.WebApiHost + $"/api/MessageReminder/GetRemindersByUserIdDate?userId={userId}&begin={begin}&end={end}";
            var list = APIInvoker.Get<List<Models.Reminder.MessageReminder>>(apiUrl);
            list = list?.OrderBy(x => x.RemindTime).ToList();

            ViewBag.UserId = userId;
            ViewBag.Nav = i;

            return View(list);
        }
    }
}