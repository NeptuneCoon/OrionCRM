using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class AppConfig
    {
        public AppConfig()
        {
            // set default value
        }

        public Logging Logging { get; set; }
        public string WebApiHost { get; set; }
        public string ApplicationHost { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// DES加密/解密密钥
        /// </summary>
        public string DesEncryptKey { get; set; }
        /// <summary>
        /// cookie过期时间(天)
        /// </summary>
        public int CookieExpire { get; set; }
        /// <summary>
        /// Memcache过期时间(秒)
        /// </summary>
        public int MemcachedExpire { get; set; }

        /// <summary>
        /// 是否开启错误信息日志记录
        /// </summary>
        public int ErrorLog { get; set; }
        /// <summary>
        /// 是否开启用户行为日志记录
        /// </summary>
        public int ActionLog { get; set; }
    }

    public struct Logging
    {
        public string IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }

    }

    public struct LogLevel
    {
        public string Default { get; set; }
    }
}
