﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp
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
        public string WebAPIHost { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }
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
