using Enyim.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebApp.Models.Account;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.ViewComponents
{
    public class LeftMenuViewComponent : ViewComponent
    {
        private IMemcachedClient _memcachedClient;
        private AppConfig _appConfig;
        public LeftMenuViewComponent(IMemcachedClient memcachedClient, IOptions<AppConfig> optionsAccessor)
        {
            _memcachedClient = memcachedClient;
            _appConfig = optionsAccessor.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 将session中保存的LeftMenuId存入ViewBag，以用来做状态保持
            ViewBag.LeftMenuId = Convert.ToInt32(HttpContext.Session.GetInt32("LeftMenuId"));

            // 从cookie中取出appUser对象
            string encryptUserInfo = HttpContext.Request.Cookies["user"];// cookie中加密的用户信息
            if(!string.IsNullOrEmpty(encryptUserInfo)) { 
                AppUserModel appUser = JsonConvert.DeserializeObject<AppUserModel>(DesEncrypt.Decrypt(encryptUserInfo, _appConfig.DesEncryptKey));
                // 取出角色下的菜单
                if (appUser != null) {
                    int topId = GetTopMenuId(appUser.RoleId);
                    IEnumerable<Models.Role.RoleMenuComplex> leftMenus = await GetLeftMenus(appUser.RoleId, topId);
                    return View(leftMenus);
                }
            }
            
            return View();
        }

        private Task<IEnumerable<Models.Role.RoleMenuComplex>> GetLeftMenus(int roleId, int topId)
        {
            // 首先判断Memcached缓存中是否存在
            var cacheLeftMenus = _memcachedClient.Get<IEnumerable<Models.Role.RoleMenuComplex>>("left_menu_" + roleId);//角色下的的二级菜单
            if (cacheLeftMenus != null) {
                // 缓存中存在，直接返回
                var displayLeftMenus = cacheLeftMenus.Where(x => x.Parent == topId).OrderBy(x => x.SortNo).AsEnumerable();//当前父菜单下的子菜单
                var result = Task.FromResult(displayLeftMenus);
                return result;
            }
            else {
                // 缓存中不存在，从数据库中获取，并写入缓存
                string apiUrl = _appConfig.WebAPIHost + "api/Role/GetComplexRoleMenusByRoleId?roleId=" + roleId;
                var roleMenus = APIInvoker.Get<List<Models.Role.RoleMenuComplex>>(apiUrl);//角色下的所有菜单

                // 写入缓存
                _memcachedClient.Add("left_menu_" + roleId, roleMenus.Where(x => x.Parent != null), int.MaxValue);

                // 获得当前角色下的Left菜单
                IEnumerable<Models.Role.RoleMenuComplex> displayLeftMenus = roleMenus.Where(x => x.Parent == topId).OrderBy(x => x.SortNo);
                

                var result = Task.FromResult(displayLeftMenus);
                return result;
            }
        }

        private Task<IEnumerable<Models.Role.RoleMenuComplex>> GetTopMenus(int roleId)
        {
            // 首先判断Memcached缓存中是否存在
            var cacheTopMenus = _memcachedClient.Get<IEnumerable<Models.Role.RoleMenuComplex>>("top_menu_" + roleId);//角色下的一级菜单
            if (cacheTopMenus != null) {
                // 缓存中存在，直接返回
                cacheTopMenus.OrderBy(x => x.SortNo).AsEnumerable();
                var result = Task.FromResult(cacheTopMenus);
                return result;
            }
            else {
                // 缓存中不存在，从数据库中获取，并写入缓存
                string apiUrl = _appConfig.WebAPIHost + "api/Role/GetComplexRoleMenusByRoleId?roleId=" + roleId;
                var roleMenus = APIInvoker.Get<List<Models.Role.RoleMenuComplex>>(apiUrl);//角色下的所有菜单

                // 获得当前角色下的Left菜单
                IEnumerable<Models.Role.RoleMenuComplex> topMenus = roleMenus.Where(x => x.Parent == null).OrderBy(x => x.SortNo);
                // 写入缓存
                _memcachedClient.Add("top_menu_" + roleId, topMenus, int.MaxValue);

                var result = Task.FromResult(topMenus);
                return result;
            }
        }

        // 获取用户当前浏览的父菜单Id
        //private int GetTopMenuId(int roleId)
        //{
        //    string encryptTopMenuId = HttpContext.Request.Cookies["top_mid"];//DES加密的TopMenuId
        //    if (!string.IsNullOrEmpty(encryptTopMenuId)) {
        //        // 如果cookie中存在，则直接返回
        //        string topMenuStr = DesEncrypt.Decrypt(encryptTopMenuId, _appConfig.DesEncryptKey);
        //        return int.Parse(topMenuStr);
        //    }
        //    else {
        //        // 如果cookie中不存在，则取用户可浏览的父菜单中的第一个，并写入缓存
        //        var topMenus = GetTopMenus(roleId);
        //        if (topMenus != null) {
        //            var firstMenu = topMenus.Result.OrderBy(x => x.SortNo).FirstOrDefault();
        //            if (firstMenu != null) {
        //                int topMenuId = firstMenu.MenuId;
        //                // 写入cookie
        //                string encryptMenuId = DesEncrypt.Encrypt(topMenuId.ToString(), _appConfig.DesEncryptKey);
        //                var cookieOptions = new CookieOptions() {
        //                    Expires = DateTime.Now.AddDays(_appConfig.CookieExpire)
        //                };
        //                HttpContext.Response.Cookies.Append("top_mid", encryptMenuId, cookieOptions);

        //                return topMenuId;
        //            } 
        //        }
        //    }
        //    return 0;
        //}

        private int GetTopMenuId(int roleId)
        {
            int topMenuId = Convert.ToInt32(HttpContext.Session.GetInt32("TopMenuId"));
            if (topMenuId > 0) {
                // 如果session中存在，则直接返回
                return topMenuId;
            }
            else {
                // 如果session中不存在，则取用户可浏览的父菜单中的第一个，并写入session
                var topMenus = GetTopMenus(roleId);
                if (topMenus != null) {
                    var firstMenu = topMenus.Result.OrderBy(x => x.SortNo).FirstOrDefault();
                    if (firstMenu != null) {
                        topMenuId = firstMenu.MenuId;
                        // 写入cookie
                        //string encryptMenuId = DesEncrypt.Encrypt(topMenuId.ToString(), _appConfig.DesEncryptKey);
                        //var cookieOptions = new CookieOptions() {
                        //    Expires = DateTime.Now.AddDays(_appConfig.CookieExpire)
                        //};
                        //HttpContext.Response.Cookies.Append("top_mid", encryptMenuId, cookieOptions);
                        HttpContext.Session.SetInt32("TopMenuId", topMenuId);

                        return topMenuId;
                    }
                }
            }
            return 0;
        }
    }
}
