using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Options
{
    public class DataAccessOptions
    {
        public ConnectionOptions ConnectionOptions { get; set; } = new ConnectionOptions();
        public SendingOptions SendingOptions { get; set; } = new SendingOptions();

        public DataAccessOptions() { }
    }
}
