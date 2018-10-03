using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Reminder
{
    public class MessageReminder
    {
        public int Id { get; set; }
        public string MsgText { get; set; }
        public DateTime RemindTime { get; set; }
        public string DisplayTime { get; set; } //提醒时间的展示性文本(eg:3小时后提醒)
        public int UserId { get; set; }
        public int Closed { get; set; }
        public int Type { get; set; }
        public int? ObjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        // 客户信息
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

        // 洽谈记录
        public List<Models.Resource.TalkRecord> TalkRecords { get; set; }
    }
}
