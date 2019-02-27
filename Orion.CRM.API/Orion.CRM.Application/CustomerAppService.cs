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
    }
}
