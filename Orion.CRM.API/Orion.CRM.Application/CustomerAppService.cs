using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

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
