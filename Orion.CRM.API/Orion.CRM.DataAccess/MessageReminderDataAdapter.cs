using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Orion.CRM.DataAccess
{
    public class MessageReminderDataAdapter : DataAdapter
    {
        public int InsertReminder(Entity.MessageReminder reminder)
        {
            if (reminder == null) return -1;

            SqlParameter[] parameters = {
                new SqlParameter("@MsgText",CheckNull(reminder.MsgText)),
                new SqlParameter("@RemindTime", reminder.RemindTime),
                new SqlParameter("@UserId", reminder.UserId),
                new SqlParameter("@Closed", reminder.Closed),
                new SqlParameter("@Type", reminder.Type),
                new SqlParameter("@ObjectId", CheckNull(reminder.ObjectId)),
                new SqlParameter("@CreateTime", reminder.CreateTime),
                new SqlParameter("@UpdateTime", reminder.UpdateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("MessageReminder", "InsertReminder", parameters);
            return identityId;
        }

        public bool UpdateReminder(Entity.MessageReminder reminder)
        {
            if (reminder == null || reminder.Id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", reminder.Id),
                new SqlParameter("@MsgText",reminder.MsgText),
                new SqlParameter("@RemindTime", reminder.RemindTime),
                new SqlParameter("@Closed", reminder.Closed),
                new SqlParameter("@UpdateTime", reminder.UpdateTime)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MessageReminder", "UpdateReminder", paramArr);
            return count > 0;
        }

        public int DeleteReminder(int id)
        {
            if (id <= 0) return 0;
            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MessageReminder", "DeleteReminder", param);
            return count;
        }

        public bool CloseReminder(int id)
        {
            if (id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", id),
                new SqlParameter("@UpdateTime", DateTime.Now)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MessageReminder", "CloseReminder", paramArr);
            return count > 0;
        }

        public IEnumerable<Entity.MessageReminder> GetRemindersByUserId(int userId)
        {
            SqlParameter param = new SqlParameter("@UserId", userId);
            var reminders = SqlMapHelper.GetSqlMapResult<Entity.MessageReminder>("MessageReminder", "GetRemindersByUserId", param);

            return reminders;
        }

        public IEnumerable<Entity.ReminderUnionTalkRecord> GetRemindersByUserIdDate(int userId, string begin, string end)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@UserId", userId),
                new SqlParameter("@BeginDate", begin),
                new SqlParameter("@EndDate", end)
            };
            var reminders = SqlMapHelper.GetSqlMapResult<Entity.ReminderUnionTalkRecord>("MessageReminder", "GetRemindersByUserIdDate", parameters);

            return reminders;
        }

        public int GetReminderCountByUserIdDate(int userId, string begin, string end)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@UserId", userId),
                new SqlParameter("@BeginDate", begin),
                new SqlParameter("@EndDate", end)
            };
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("MessageReminder", "GetReminderCountByUserIdDate", parameters);

            return count;
        }

        public IEnumerable<Entity.MessageReminder> GetRemindersByObjectId(int type, int? objectId)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@Type", type),
                new SqlParameter("@ObjectId", CheckNull(objectId))
            };
            var reminders = SqlMapHelper.GetSqlMapResult<Entity.MessageReminder>("MessageReminder", "GetRemindersByObjectId", parameters);

            return reminders;
        }
    }
}
