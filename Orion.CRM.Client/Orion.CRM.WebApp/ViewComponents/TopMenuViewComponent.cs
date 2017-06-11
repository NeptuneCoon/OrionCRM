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
    public class TopMenuViewComponent : ViewComponent
    {
        private IMemcachedClient _memcachedClient;
        private AppConfig _appConfig;
        public TopMenuViewComponent(IMemcachedClient memcachedClient, IOptions<AppConfig> optionsAccessor)
        {
            _memcachedClient = memcachedClient;
            _appConfig = optionsAccessor.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 将session中保存的TopMenuId存入ViewBag，以用来做状态保持
            ViewBag.TopMenuId = Convert.ToInt32(HttpContext.Session.GetInt32("TopMenuId"));

            // 从cookie中取出appUser对象
            string encryptUserInfo = HttpContext.Request.Cookies["user"];// cookie中加密的用户信息
            if (!string.IsNullOrEmpty(encryptUserInfo)) {
                AppUserModel appUser = JsonConvert.DeserializeObject<AppUserModel>(DesEncrypt.Decrypt(encryptUserInfo, _appConfig.DesEncryptKey));
                // 取出角色下的菜单
                if (appUser != null) {
                    IEnumerable<Models.Role.RoleMenuComplex> topMenus = await GetTopMenus(appUser.RoleId);
                    return View(topMenus);
                }
            }
            
            return View();
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
    }
}
