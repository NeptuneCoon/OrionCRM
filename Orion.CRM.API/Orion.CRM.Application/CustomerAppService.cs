using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Orion.CRM.Application
{
    public class CustomerAppService
    {
        private CustomerDataAdapter adapter = new CustomerDataAdapter();
        public int InsertCustomer(Entity.Customer customer)
        {
            return adapter.InsertCustomer(customer);
        }

        public bool UpdateCustomer(Entity.Customer customer)
        {
            return adapter.UpdateCustomer(customer);
        }

        public bool DeleteCustomer(int id, string webRootPath)
        {
            if (id <= 0) return false;
            if (string.IsNullOrEmpty(webRootPath)) return false;

            bool res = true;

            // step0.在删除客户之前，先查询出服务记录<准备工作>
            List<Entity.CustomerServiceRecord> records = GetServiceRecordsByCustomerId(id)?.ToList();

            // step1.先删除客户Customer
            res = adapter.DeleteCustomer(id);

            // step2.如果客户删除成功，则删除其服务记录中的图片资源
            try
            {
                if (records != null && records.Count > 0)
                {
                    foreach (var record in records)
                    {
                        if (!string.IsNullOrEmpty(record.Images))
                        {
                            string[] imageArr = record.Images.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (imageArr.Length > 0)
                            {
                                foreach (var relative_path in imageArr)
                                {
                                    string fullPath = $@"{webRootPath}{relative_path}";
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // log...
            }

            return res;
        }

        public Entity.Customer GetCustomerById(int id)
        {
            return adapter.GetCustomerById(id);
        }

        public IEnumerable<Entity.Customer> GetCustomersByCondition(Entity.CustomerSearchParams param)
        {
            return adapter.GetCustomersByCondition(param);
        }

        public int GetCustomersCountByCondition(Entity.CustomerSearchParams param)
        {
            return adapter.GetCustomersCountByCondition(param);
        }

        public int AssignServiceUser(string customerIds, int userId)
        {
            if (string.IsNullOrEmpty(customerIds) || userId <= 0) return 0;

            return adapter.AssignServiceUser(customerIds, userId);
        }

        public IEnumerable<Entity.Customer> GetCustomersByZone(int pid, int bid, string z1)
        {
            if (pid <= 0 || bid <= 0) return null;
            return adapter.GetCustomersByZone(pid, bid, z1);
        }




        // 以下3个方法是[客户服务记录CustomerServiceRecord表]有关操作
        public int InsertServiceRecord(Entity.CustomerServiceRecord record)
        {
            return adapter.InsertServiceRecord(record);
        }

        public bool DeleteServiceRecord(int id)
        {
            if (id <= 0) return false;
            return adapter.DeleteServiceRecord(id);
        }


        public IEnumerable<Entity.CustomerServiceRecord> GetServiceRecordsByCustomerId(int customerId)
        {
            if (customerId <= 0) return null;
            return adapter.GetServiceRecordsByCustomerId(customerId);
        }

        public Entity.CustomerServiceRecord CustomerServiceRecord(int id)
        {
            if (id <= 0) return null;
            return adapter.CustomerServiceRecord(id);
        }
    }
}
