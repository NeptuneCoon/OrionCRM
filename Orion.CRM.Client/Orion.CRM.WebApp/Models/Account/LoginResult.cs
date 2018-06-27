using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Account
{
    public class LoginResult
    {
        public LoginResult()
        {
        }

        public LoginResult(int errorCode, string userName)
        {
            ErrorCode = errorCode;
            UserName = userName;
        }
        /// <summary>
        /// 错误代码(200=success,101=用户已被禁用,102=密码有误,103=用户名不存在)
        /// </summary>
        public int ErrorCode { get; set; }

        public string UserName { get; set; }
    }
}
