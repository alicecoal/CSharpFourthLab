using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager.Utilities;
using OptionsManager;
using DataAccessLayer;
using DataAccessLayer.Utilities;
using DataAccessLayer.Options;
using DataAccessLayer.Models;
using ServiceLayer;
using Logger;

namespace DataManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Parser.Parser parser = new Parser.Parser();
            IValidator validator = new Validator();
            OptionsManager<DataAccessOptions> options =
                new OptionsManager<DataAccessOptions>(appDirectory, parser, validator);

            LoggerOptions loggerOptions = new LoggerOptions();
            loggerOptions.ConnectionOptions = options.GetOptions<ConnectionOptions>() as ConnectionOptions;
            Logger.Logger logger = new Logger.Logger(loggerOptions);

            logger.Log(options.LogString);

            ServiceLayer.ServiceLayer SL = new ServiceLayer.ServiceLayer(
                options.GetOptions<ConnectionOptions>() as ConnectionOptions);
            SendingOptions sendingOptions = options.GetOptions<SendingOptions>() as SendingOptions;

            logger.Log("DataManager is retrieving data");
            PersonData personInfo = SL.GetPersonInfo();
            logger.Log("DataManager retrieved data");
           
            XmlGeneratorService xmlGenerator = new XmlGeneratorService(sendingOptions);
            xmlGenerator.SerializeXML(personInfo);
            logger.Log("DataManager sent data to server");

        }
    }
}
