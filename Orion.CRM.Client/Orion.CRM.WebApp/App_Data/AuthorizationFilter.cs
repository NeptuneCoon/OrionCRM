using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    public class AuthorizationFilter: IAuthorizationFilter
    {
        private readonly AppConfig _appConfig;
        private IMemoryCache _memoryCache;
        public AuthorizationFilter(IOptions<AppConfig> optionsAccessor, IMemoryCache memoryCache)
        {
            _appConfig = optionsAccessor.Value;
            _memoryCache = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var query = context.Filters.FirstOrDefault(x => x.ToString().IndexOf("AnonymousFilter") >= 0);

            if (query == null) {
                bool authorized = true;
                
                string userInCookie = context.HttpContext.Request.Cookies["user"];// DES加密后的用户信息
                string tokenInCookie = context.HttpContext.Request.Cookies["token"];// cookie中的token
                if(userInCookie == null || tokenInCookie == null) {
                    authorized = false;
                }
                else {
                    string decryptUser = DesEncrypt.Decrypt(userInCookie, _appConfig.DesEncryptKey);
                    var cookieUser = JsonConvert.DeserializeObject<Models.Account.AppUserModel>(decryptUser);

                    // 服务器端缓存中的token
                    object tokenInCache = _memoryCache.Get("token_" + cookieUser.Id);

                    /*
                    // 比较token是否一致
                    if (tokenInCache == null) {
                        // 服务器端token丢失，有可能是服务器重启等非用户原因导致
                        // 为避免cookie没过期但需要用户重新登录的问题，这里重新去服务器获取并生成token，然后再作比对
                        var appUser = APIInvoker.Get<Models.Account.AppUserModel>(_appConfig.WebApiHost + "/api/AppUser/GetUserById?id=" + cookieUser.Id);
                        if (appUser != null) {
                            string tokenContent = appUser.UserName + "," + appUser.Password;
                            string token = DesEncrypt.Encrypt(tokenContent, _appConfig.DesEncryptKey);
                            if (token != tokenInCookie) {
                                authorized = false;
                            }
                            // 将token写入服务器端缓存
                            var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                            _memoryCache.Set("token_" + cookieUser.Id, token, cacheOptons);
                        }
                        else {
                            authorized = false;
                        }
                    }
                    else {
                        // cookie中的token已过期或没过期但用户密码已更改
                        if (tokenInCookie == null || tokenInCookie != tokenInCache.ToString()) {
                            authorized = false;
                        }
                    }
                    */
                    if (tokenInCache == null) authorized = false;//重启服务器以让用户都掉线，然后清除用户cookie
                }
                if (!authorized) {
                    context.HttpContext.Response.Redirect(_appConfig.ApplicationHost + "/Account/Login");
                    context.Result = new EmptyResult();
                }
            }
        }
    }
}
