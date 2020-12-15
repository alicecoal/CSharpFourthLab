using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class Logger
    {
        public LoggerOptions LoggerOptions { get; set; }
        DataAccessLayer.DataAccessLayer DAL;

        public Logger(LoggerOptions loggerOptions)
        {
            LoggerOptions = loggerOptions;
            DAL = new DataAccessLayer.DataAccessLayer(loggerOptions.ConnectionOptions);
        }

        public void Log(string msg)
        {
            if (LoggerOptions.NeedToLog) 
            {
                DAL.Log(DateTime.Now, msg);
            }
        }
    }
}
