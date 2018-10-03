using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.Application;

namespace Orion.CRM.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class MessageReminderController : Controller
    {
        private MessageReminderAppService service = new MessageReminderAppService();


        [HttpPost]
        public APIDataResult InsertReminder([FromBody]Entity.MessageReminder reminder)
        {
            try {
                int identityId = service.InsertReminder(reminder);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateReminder([FromBody]Entity.MessageReminder reminder)
        {
            try {
                bool res = service.UpdateReminder(reminder);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteReminder(int id)
        {
            try {
                int count = service.DeleteReminder(id);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult CloseReminder(int id)
        {
            try {
                bool res = service.CloseReminder(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRemindersByUserId(int userId)
        {
            try {
                IEnumerable<Entity.MessageReminder> data = service.GetRemindersByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, data);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 根据UserId和提醒日期获取可用提醒
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult GetRemindersByUserIdDate(int userId, string begin, string end)
        {
            try {
                IEnumerable<Entity.MessageReminder> data = service.GetRemindersByUserIdDate(userId, begin, end);
                APIDataResult dataResult = new APIDataResult(200, data);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetReminderCountByUserIdDate(int userId, string begin, string end)
        {
            try {
                int count = service.GetReminderCountByUserIdDate(userId, begin, end);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRemindersByObjectId(int type, int? objectId)
        {
            try {
                IEnumerable<Entity.MessageReminder> data = service.GetRemindersByObjectId(type, objectId);
                APIDataResult dataResult = new APIDataResult(200, data);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}