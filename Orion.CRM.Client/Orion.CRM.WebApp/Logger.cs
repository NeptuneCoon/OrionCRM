using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp
{
    /// <summary>
    /// 临时日志记录工具(调试代码用)
    /// </summary>
    public class Logger
    {

        public static void Write(string content)
        {
            try {
                string path = System.IO.Directory.GetCurrentDirectory();
                content = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + content + "\r\n";
                System.IO.File.AppendAllText(@"E:\website\crm_site\site_log.txt", content, System.Text.Encoding.UTF8);
            }
            catch {
                // ...
            }
        }

        public static void ErrorLog(string apiHost,string className, string methodName, string errorMsg, string parameters = "")
        {
            try {
                string apiUrl = apiHost + "/api/CRMLog/InsertErrorLog";
                var errorLog = new
                {
                    Origin = 1,
                    ClassName = className,
                    MethodName = methodName,
                    ErrorMsg = errorMsg,
                    Parameters = parameters
                };

                long cnt = APIInvoker.Post<long>(apiUrl, errorLog);
            }
            catch(Exception ex) {

            }
        }

        public static void ActionLog(string apiHost, Models.CRMLog.ActionLog log)
        {
            try {
                string apiUrl = apiHost + "/api/CRMLog/InsertActionLog";
                long cnt = APIInvoker.Post<long>(apiUrl, log);
            }
            catch (Exception ex) {

            }
        }

        public static void LoginLog(string apiHost, Models.CRMLog.LoginLog log)
        {
            try {
                string apiUrl = apiHost + "/api/CRMLog/InsertLoginLog";
                long cnt = APIInvoker.Post<long>(apiUrl, log);
            }
            catch (Exception ex) {

            }
        }
    }
}
