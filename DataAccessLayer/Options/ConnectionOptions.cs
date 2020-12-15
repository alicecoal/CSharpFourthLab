using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Options
{
    public class ConnectionOptions
    {
        public string DataSource { get; set; } = @".\SQLEXPRESS";
        public string InitialCatalog { get; set; } = "AdventureWorks2017";
        public bool IntegratedSecurity { get; set; } = true;

        public ConnectionOptions() { }
    }
}
