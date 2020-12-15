using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Options;

namespace Logger
{
    public class LoggerOptions
    {
        public ConnectionOptions ConnectionOptions { get; set; } =
            new ConnectionOptions();
        public bool NeedToLog { get; set; } = true;

        public LoggerOptions()
        {
        }
    }
}
