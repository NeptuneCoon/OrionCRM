using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// 分页option属性
    /// </summary>
    public class PagerOption
    {
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }

        public string QueryString { get; set; }

        /// <summary>
        /// 样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }
    }

    /// <summary>
    /// 分页标签
    /// </summary>
    public class PagerTagHelper : TagHelper
    {
        public PagerOption PagerOption { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            if (PagerOption.TotalCount <= 0) { return; }

            //总页数
            var totalPage = PagerOption.TotalCount / PagerOption.PageSize + (PagerOption.TotalCount % PagerOption.PageSize > 0 ? 1 : 0);
            if (totalPage <= 0) { return; }
            //当前路由地址
            if (string.IsNullOrEmpty(PagerOption.RouteUrl)) {

                //PagerOption.RouteUrl = helper.ViewContext.HttpContext.Request.RawUrl;
                if (!string.IsNullOrEmpty(PagerOption.RouteUrl)) {

                    var lastIndex = PagerOption.RouteUrl.LastIndexOf("/");
                    PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
                }
            }
            PagerOption.RouteUrl = PagerOption.RouteUrl.TrimEnd('/');

            //构造分页样式
            var sbPage = new StringBuilder(string.Empty);
            switch (PagerOption.StyleNum) {
                case 2: {
                        break;
                    }
                default: {
                        #region 默认样式

                        sbPage.Append("<nav>");
                        sbPage.Append("<ul class=\"pager-ul\">");
                        sbPage.AppendFormat("<li><a href=\"{0}&{1}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>",
                                                "?id=" + (PagerOption.PageIndex - 1 <= 0 ? 1 : PagerOption.PageIndex - 1),
                                                PagerOption.QueryString
                                                );

                        for (int i = 1; i <= totalPage; i++) {

                            sbPage.AppendFormat("<li {1}><a href=\"{2}/{0}\">{0}</a></li>",
                                i,
                                i == PagerOption.PageIndex ? "class=\"active\"" : "",
                                PagerOption.RouteUrl + PagerOption.QueryString);

                        }

                        sbPage.Append("<li>");
                        sbPage.AppendFormat("<a href=\"{0}/{1}\" aria-label=\"Next\">",
                                            PagerOption.RouteUrl + PagerOption.QueryString,
                                            PagerOption.PageIndex + 1 > totalPage ? PagerOption.PageIndex : PagerOption.PageIndex + 1);
                        sbPage.Append("<span aria-hidden=\"true\">&raquo;</span>");
                        sbPage.Append("</a>");
                        sbPage.Append("</li>");
                        sbPage.Append("</ul>");
                        sbPage.Append("</div>");
                        #endregion
                    }
                    break;
            }

            output.Content.SetHtmlContent(sbPage.ToString());
            //output.TagMode = TagMode.SelfClosing;
            //return base.ProcessAsync(context, output);
        }
    }
}
