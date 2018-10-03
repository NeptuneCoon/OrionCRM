using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Orion.CRM.Application
{
    public class MessageReminderAppService
    {
        private MessageReminderDataAdapter adapter = new MessageReminderDataAdapter();

        public int InsertReminder(Entity.MessageReminder reminder)
        {
            return adapter.InsertReminder(reminder);
        }

        public bool UpdateReminder(Entity.MessageReminder reminder)
        {
            return adapter.UpdateReminder(reminder);
        }

        public int DeleteReminder(int id)
        {
            return adapter.DeleteReminder(id);
        }

        public bool CloseReminder(int id)
        {
            return adapter.CloseReminder(id);
        }

        public IEnumerable<Entity.MessageReminder> GetRemindersByUserId(int userId)
        {
            var reminders = adapter.GetRemindersByUserId(userId);
            return ReminderTimeFormat(reminders);
        }

        /// <summary>
        /// 根据UserId和提醒日期获取可用提醒
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Entity.MessageReminder> GetRemindersByUserIdDate(int userId, string begin, string end)
        {
            var unionReminders = adapter.GetRemindersByUserIdDate(userId, begin, end);//混合数据
            // 重新组合成资源和洽谈记录一对多的实体
            if (unionReminders == null) return null;

            List<Entity.MessageReminder> data = new List<Entity.MessageReminder>();//实际返回的数据

            var groups = unionReminders.GroupBy(x => x.Id).ToList();
            if (groups != null && groups.Count > 0) {
                foreach(var group in groups) {
                    Entity.MessageReminder item = new Entity.MessageReminder();

                    var union = unionReminders.FirstOrDefault(x => x.Id == group.Key);//混合了Reminder表和TalkRecord表的数据实体

                    // 提醒的基本信息
                    item.Id = union.Id;
                    item.MsgText = union.MsgText;
                    item.RemindTime = union.RemindTime;
                    item.UserId = union.UserId;
                    item.Closed = union.Closed;
                    item.Type = union.Type;
                    item.ObjectId = union.ObjectId;
                    item.CustomerName = union.CustomerName;
                    item.Address = union.Address;
                    item.Contact = union.Mobile;//联系方式默认为手机号码

                    // 洽谈记录的信息
                    var records = unionReminders.Where(x => x.Id == group.Key && x.ResourceId == union.ObjectId)?.OrderByDescending(x=>x.CreateTime).ToList();//包含洽谈记录的混合模型
                    if (records != null && records.Count > 0) {
                        item.TalkRecords = new List<Entity.TalkRecord>();
                        foreach (var record in records) {
                            item.TalkRecords.Add(new Entity.TalkRecord() {
                                ResourceId = record.ResourceId,
                                CreateTime = record.CreateTime,
                                TalkResult = record.TalkResult
                            });
                        }
                    }
                    data.Add(item);
                }
            }
            return ReminderTimeFormat(data);
        }

        public int GetReminderCountByUserIdDate(int userId, string begin, string end)
        {
            return adapter.GetReminderCountByUserIdDate(userId, begin, end);
        }

        public IEnumerable<Entity.MessageReminder> GetRemindersByObjectId(int type, int? objectId)
        {
            var reminders = adapter.GetRemindersByObjectId(type, objectId);
            return ReminderTimeFormat(reminders);
        }

        /// <summary>
        /// 提醒时间人性化处理
        /// 将会把时间处理成：xx天xx小时xx分钟后提醒
        /// </summary>
        /// <param name="reminders"></param>
        /// <returns></returns>
        IEnumerable<Entity.MessageReminder> ReminderTimeFormat(IEnumerable<Entity.MessageReminder> reminders)
        {
            if (reminders == null) return null;
            // 提醒时间的人性化展示
            foreach (var reminder in reminders) {
                if (reminder.Closed == 1) continue;

                TimeSpan ts = reminder.RemindTime - DateTime.Now;
                
                if (ts.TotalMinutes > 0) {
                    string displayTime = "";
                    if (ts.Days > 0) {
                        displayTime += ts.Days + "天";
                    }
                    if (ts.Hours > 0) {
                        displayTime += ts.Hours + "小时";
                    }
                    if (ts.Minutes > 0) {
                        displayTime += ts.Minutes + "分钟";
                    }
                    if (displayTime.Length > 0) {
                        displayTime += "后提醒";
                    }
                    reminder.DisplayTime = displayTime;
                }
            }
            return reminders;
        }
    }
}
