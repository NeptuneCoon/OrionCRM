using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class CRMLogAppService
    {
        private CRMLogDataAdapter adapter = new CRMLogDataAdapter();
        public long InsertActionLog(Entity.CRMLog.ActionLog log)
        {
            return adapter.InsertActionLog(log);
        }
        public long InsertErrorLog(Entity.CRMLog.ErrorLog log)
        {
            return adapter.InsertErrorLog(log);
        }

        public long InsertLoginLog(Entity.CRMLog.LoginLog log)
        {
            return adapter.InsertLoginLog(log);
        }
    }
}
