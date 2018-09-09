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
            string path = System.IO.Directory.GetCurrentDirectory();
            content = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + content + "\r\n";
            System.IO.File.AppendAllText(@"E:\website\crm_site\site_log.txt", content, System.Text.Encoding.UTF8);
        }
    }
}
