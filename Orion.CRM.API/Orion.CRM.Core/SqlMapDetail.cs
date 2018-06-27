using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Orion.CRM.Core
{
    public class SqlMapDetail
    {
        public string SqlName { get; set; }

        [XmlAttribute("TableName")]
        public string TableName { get; set; }
        public string OriginalSqlString { get; set; }
        public string DBConnectionName { get; set; }
        public string CommandType { get; set; }

        public object Clone()
        {
            return new SqlMapDetail {
                CommandType = this.CommandType,
                DBConnectionName = this.DBConnectionName,
                OriginalSqlString = this.OriginalSqlString,
                SqlName = this.SqlName
            };
        }
    }
}
