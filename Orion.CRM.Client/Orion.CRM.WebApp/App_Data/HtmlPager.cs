using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    public class HtmlPagerTagHelper : TagHelper
    {
        public PagerOption PagerOption { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(PagerOption.ClassName)) {
                PagerOption.ClassName = "pager-ul";
            }

            int pageIndex = PagerOption.PageIndex;
            int pageMax = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(PagerOption.TotalCount) / PagerOption.PageSize));

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"" + PagerOption.ClassName + "\">");
            // 首页
            if (pageIndex > 1) {
                string url = PagerOption.RouteUrl + IndexReplace(PagerOption.QueryString, 1);
                sb.Append($"<li {(pageIndex == 1 ? "class='active'" : "")}><a href=\"{url}\">首页</a></li>");
            }
            // 上一页
            if (pageIndex == 1) {
                sb.Append($"<li><span>上一页</span></li>");
            }
            else {
                string prevUrl = PagerOption.RouteUrl + IndexReplace(PagerOption.QueryString, pageIndex - 1);
                sb.Append($"<li><a href=\"{prevUrl}\">上一页</a></li>");
            }

            // 页码(取10页展示)
            int beginIndex = Math.Max(1, pageIndex - 5);
            int endIndex = Math.Min(pageMax, beginIndex + 9);
            for (int i = beginIndex; i <= endIndex; i++) {
                string url = PagerOption.RouteUrl + IndexReplace(PagerOption.QueryString, i);
                sb.Append($"<li {(pageIndex == i ? "class='active'" : "")}><a href=\"{url}\">{i}</a></li>");
            }

            // 下一页
            if (pageIndex < pageMax) {
                string nextUrl = PagerOption.RouteUrl + IndexReplace(PagerOption.QueryString, pageIndex + 1);
                sb.Append($"<li><a href=\"{nextUrl}\">下一页</a></li>");
            }
            else {
                sb.Append($"<li><span>下一页</span></li>");
            }
            // 末页
            if (pageIndex < pageMax) {
                string lastUrl = PagerOption.RouteUrl + IndexReplace(PagerOption.QueryString, pageMax);
                sb.Append($"<li {(pageIndex == pageMax ? "class='active'" : "")}><a href=\"{lastUrl}\">末页</a></li>");
            }
            sb.Append("</ul>");

            output.Content.SetHtmlContent(sb.ToString());
        }

        private string IndexReplace(string queryString, int newPageIndex)
        {
            if (string.IsNullOrEmpty(queryString)) {
                return "?pi=" + newPageIndex;
            }

            //Dictionary<string, string> dict = new Dictionary<string, string>();
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string[] arr = queryString.Split(new char[] { '&', '?' });//inc=1  pageindex=
            foreach(var item in arr) {
                string[] paramInfo = item.Split('=');
                if (paramInfo.Length == 2) {
                    string key = paramInfo[0];
                    string value = paramInfo[1];
                    if (key.ToLower() == "pi") {
                        value = newPageIndex.ToString();
                    }
                    //dict.Add(key, value);
                    list.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            /*
            #region 重新组合成QueryString old
            // 重新组合成QueryString
            string newQueryString = "?";
            int i = 0;
            foreach (var item in dict) {
                if (i < dict.Count - 1) {
                    newQueryString += item.Key + "=" + item.Value + "&";
                }
                else {
                    newQueryString += item.Key + "=" + item.Value;
                }
                i++;
            } 
            #endregion
            */
            // 重新组合成QueryString
            string newQueryString = "?";
            int i = 0;
            foreach (var item in list) {
                if (i < list.Count - 1) {
                    newQueryString += item.Key + "=" + item.Value + "&";
                }
                else {
                    newQueryString += item.Key + "=" + item.Value;
                }
                i++;
            }

            //if (!dict.ContainsKey("pi")) {
            //    newQueryString += "&pi=" + newPageIndex;
            //}
            bool hasPiKey = false;
            foreach(var item in list) {
                if(item.Key == "pi") {
                    hasPiKey = true;
                    break;
                }
            }
            if (!hasPiKey) {
                newQueryString += "&pi=" + newPageIndex;
            }
            return newQueryString;
        }

    }

    public class PagerOption
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 每页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 路由地址(/Controller/Action)
        /// </summary>
        public string RouteUrl { get; set; }

        /// <summary>
        /// 路由后面所带的参数(&param1=123&param2=456...)
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string ClassName { get; set; }
    }
}
