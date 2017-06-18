using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public abstract class DataAdapter
    {
        /// <summary>
        /// 参数null值检测
        /// 如果参数为null，返回DBNull.value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object CheckNull(object item)
        {
            if (item == null) {
                return DBNull.Value;
            }
            else {
                return item;
            }
        }
    }
}
