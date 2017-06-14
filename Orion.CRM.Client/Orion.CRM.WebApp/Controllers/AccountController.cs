using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ��¼ע�������
    /// </summary>
    public class AccountController : BaseController
    {
        private IMemoryCache _memoryCache;
        public AccountController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [TypeFilter(typeof(AnonymousFilter))]
        public IActionResult Login()
        {
            Models.Account.LoginViewModel viewModel = new Models.Account.LoginViewModel();

            if (TempData["ErrorInfo"] != null) { 
                Models.Account.LoginResult loginResult = JsonConvert.DeserializeObject<Models.Account.LoginResult>(TempData["ErrorInfo"].ToString());
                if(loginResult != null) {
                    ViewBag.ErrorCode = loginResult.ErrorCode;
                    viewModel.UserName = loginResult.UserName;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [TypeFilter(typeof(AnonymousFilter))]
        public IActionResult LoginHandler(Models.Account.LoginViewModel viewModel)
        {
            if (viewModel != null) {
                var apiUrl = _AppConfig.WebAPIHost + "api/AppUser/GetUserByUserName?userName=" + viewModel.UserName;
                var appUser = APIInvoker.Get<Models.Account.AppUserModel>(apiUrl);

                if (appUser != null) {
                    if (appUser.Enable == 0) {
                        // �û��ѱ�����
                        var loginResult = new Models.Account.LoginResult(101, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else if(appUser.Password != Md5Encrypt.Md5Bit32(viewModel.Password)) {
                        // ��������
                        var loginResult = new Models.Account.LoginResult(102, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else {
                        // ����һ��token��д��cookie����token���û���+������Ϊ�û�ƾ��
                        string tokenContent = appUser.UserName + "," + appUser.Password;
                        string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                        var cookieOptions = new CookieOptions(){
                            Expires = DateTime.Now.AddDays(_AppConfig.CookieExpire)
                        };
                        Response.Cookies.Append("token", token, cookieOptions);

                        // ��user��ϢDES���ܺ�д��cookie
                        string userInfo = DesEncrypt.Encrypt(JsonConvert.SerializeObject(appUser), _AppConfig.DesEncryptKey);
                        Response.Cookies.Append("user", userInfo, cookieOptions);

                        // ��token�����ڷ������˻�����(��������)
                        var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                        _memoryCache.Set("token_" + appUser.Id, token, cacheOptons);
                        // ���û���Ϣ�����ڷ������˻�����(��������)
                        _memoryCache.Set("user_" + appUser.Id, appUser, cacheOptons);

                        return RedirectToAction("List", "Resource");
                    }
                }
                else {
                    // �û���������
                    var loginResult = new Models.Account.LoginResult(103, viewModel.UserName);
                    TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                    return RedirectToAction("Login", "Account");
                }
            }

            return RedirectToAction("List", "Resource");
        }
    }
}