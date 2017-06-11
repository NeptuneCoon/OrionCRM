using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebAPI
{
    public class APIDataResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public APIDataResult()
        {
        }

        public APIDataResult(int errorStatus)
        {
            Status = errorStatus;
            Message = string.Empty;
            Data = null;
        }

        //public ApiResult(int status, string message, object data)
        //{
        //    Status = status;
        //    Message = message;
        //    Data = data;
        //} 

        public APIDataResult(int status, object data, string message = "")
        {
            Status = status;
            Message = message;
            Data = JsonConvert.SerializeObject(data);
        }
    }
}
