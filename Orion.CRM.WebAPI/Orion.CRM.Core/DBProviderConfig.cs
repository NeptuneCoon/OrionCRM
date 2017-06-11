using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Orion.CRM.Core
{
    public class DBProviderConfig
    {
        public List<DBProvider> DBProviders { get; set; }
    }

    public class DBProvider
    {
        [XmlAttribute("category")]
        public string Category { get; set; }

        public List<DBConnection> DBConnections { get; set; }
    }

    public class DBConnection
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("connection")]
        public string Connection { get; set; }
    }
}
