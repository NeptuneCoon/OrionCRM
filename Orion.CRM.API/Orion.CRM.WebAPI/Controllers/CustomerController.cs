﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.Application;

namespace Orion.CRM.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CustomerController : Controller
    {
        private CustomerAppService service = new CustomerAppService();

        [HttpPost]
        public APIDataResult InsertCustomer([FromBody]Entity.Customer customer)
        {
            try {
                int identityId = service.InsertCustomer(customer);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateCustomer([FromBody]Entity.Customer customer)
        {
            try {
                bool res = service.UpdateCustomer(customer);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetCustomerById(int id)
        {
            try
            {
                Entity.Customer res = service.GetCustomerById(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        /// <summary>
        /// 根据筛选条件查询客户
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetCustomersByCondition([FromBody]Entity.CustomerSearchParams param)
        {
            try
            {
                IEnumerable<Entity.Customer> resources = service.GetCustomersByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, resources);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 根据筛选条件获取符合条件的客户个数
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetCustomersCountByCondition([FromBody]Entity.CustomerSearchParams param)
        {
            try
            {
                int count = service.GetCustomersCountByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        [HttpGet]
        public APIDataResult AssignServiceUser(string customerIds, int userId)
        {
            try
            {
                int count = service.AssignServiceUser(customerIds, userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetCustomersByZone1(string agentZone1)
        {
            try
            {
                IEnumerable<Entity.Customer> resources = service.GetCustomersByZone1(agentZone1);
                APIDataResult dataResult = new APIDataResult(200, resources);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }



        [HttpPost]
        public APIDataResult InsertServiceRecord([FromBody]Entity.CustomerServiceRecord record)
        {
            try
            {
                int identityId = service.InsertServiceRecord(record);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteServiceRecord(int id)
        {
            try
            {
                bool res = service.DeleteServiceRecord(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetServiceRecordsByCustomerId(int customerId)
        {
            try
            {
                var result = service.GetServiceRecordsByCustomerId(customerId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult CustomerServiceRecord(int id)
        {
            try
            {
                var result = service.CustomerServiceRecord(id);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex)
            {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

    }
}