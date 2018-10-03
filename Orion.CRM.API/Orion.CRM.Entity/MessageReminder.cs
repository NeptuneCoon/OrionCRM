using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
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
        public List<Entity.TalkRecord> TalkRecords { get; set; }
    }

    public class ReminderUnionTalkRecord
    {
        // MessageReminder表
        public int Id { get; set; }
        public string MsgText { get; set; }
        public DateTime RemindTime { get; set; }
        public string DisplayTime { get; set; } //提醒时间的展示性文本(eg:3小时后提醒)
        public int UserId { get; set; }
        public int Closed { get; set; }
        public int Type { get; set; }
        public int? ObjectId { get; set; }

        // Resource表
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }

        // TalkRecord表
        public int ResourceId { get; set; }
        public string TalkResult { get; set; }
        /// <summary>
        /// 洽谈记录的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
