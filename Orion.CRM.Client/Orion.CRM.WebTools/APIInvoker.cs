﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Orion.CRM.WebTools
{
    public class APIInvoker
    {
        /// <summary>
        /// 执行一个get请求，并获取结果，该方法会直接返回APIDataResult的Data属性
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        public static T Get<T>(string url)
        {
            string responseText = HttpGet(url);

            if(!string.IsNullOrEmpty(responseText)) {
                APIDataResult dataResult = JsonConvert.DeserializeObject<APIDataResult>(responseText);
                if (dataResult != null && dataResult.Status == 200) {
                    if (!string.IsNullOrEmpty(dataResult.Data)) {
                        T data = JsonConvert.DeserializeObject<T>(dataResult.Data);
                        return data;
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 执行一个post请求，并获取结果，该方法会直接返回APIDataResult的Data属性
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="requestObj">参数对象</param>
        /// <returns></returns>
        public static T Post<T>(string url, object requestObj)
        {
            string requestData = JsonConvert.SerializeObject(requestObj);
            string responseText = HttpPost(url, requestData);

            if (!string.IsNullOrEmpty(responseText)) {
                APIDataResult dataResult = JsonConvert.DeserializeObject<APIDataResult>(responseText);
                if (dataResult != null && dataResult.Status == 200) {
                    if (!string.IsNullOrEmpty(dataResult.Data)) {
                        T data = JsonConvert.DeserializeObject<T>(dataResult.Data);
                        return data;
                    }
                }
            }
            return default(T);
        }

        // get
        private static string HttpGet(string url)
        {
            string responseText = "";
            try {
                responseText = new HttpClient().GetStringAsync(url).Result;
            }
            catch {
                //CommonLogger.LogError("调用API:" + url + "时出错！", ex);
            }
            return responseText;
        }

        // post
        private static string HttpPost(string url, string requestData)
        {
            string responseText = "";
            try {
                StringContent content = new StringContent(requestData, System.Text.Encoding.UTF8, "application/json");
                responseText = new HttpClient().PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            }
            catch {
                //CommonLogger.LogError("调用API:" + url + "时出错！", ex);
            }
            return responseText;
        }
    }
}
