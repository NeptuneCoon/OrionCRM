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
        private AppConfig _appConfig;
        public LeftMenuViewComponent(IOptions<AppConfig> optionsAccessor)
        {
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
                    int topId = await GetTopMenuId(appUser.RoleId);
                    IEnumerable<Models.Role.RoleMenuComplex> leftMenus = await AppDTO.GetLeftMenus(appUser.RoleId, topId);
                    return View(leftMenus);
                }
            }
            
            return View();
        }

        private async Task<int> GetTopMenuId(int roleId)
        {
            int topMenuId = Convert.ToInt32(HttpContext.Session.GetInt32("TopMenuId"));
            if (topMenuId > 0) {
                // 如果session中存在，则直接返回
                return topMenuId;
            }
            else {
                // 如果session中不存在，则取用户可浏览的父菜单中的第一个，并写入session
                var topMenus = await AppDTO.GetTopMenus(roleId);
                if (topMenus != null) {
                    var firstMenu = topMenus.OrderBy(x => x.SortNo).FirstOrDefault();
                    if (firstMenu != null) {
                        topMenuId = firstMenu.MenuId;
                        HttpContext.Session.SetInt32("TopMenuId", topMenuId);

                        return topMenuId;
                    }
                }
            }
            return 0;
        }
    }
}
