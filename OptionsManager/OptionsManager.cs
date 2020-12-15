using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;

namespace OptionsManager
{
    public class OptionsManager<T> where T : new()
    {
        T defaultOptions;
        T jsonOptions;
        T xmlOptions;

        public string LogString { get; } = "";

        bool isJsonConfigured = false;
        bool isXmlConfigured = false;

        public OptionsManager(string path, Parser.Parser parser, IValidator validator)
        {
            defaultOptions = new T();
            string options;

            try
            {
                using (StreamReader sr = new StreamReader($"{path}appsettings.json"))
                {
                    options = sr.ReadToEnd();
                }

                jsonOptions = parser.DeserializeJson<T>(options);
                validator.Validate(jsonOptions);
                isJsonConfigured = true;
                LogString += "appsettings.json is loaded.\n";
            }
            catch (Exception ex)
            {
                isJsonConfigured = false;
                LogString += ex.Message;
            }

            try
            {
                using (StreamReader sr = new StreamReader($"{path}config.xml"))
                {
                    options = sr.ReadToEnd();
                }

                xmlOptions = parser.DeserializeXML<T>(options);
                validator.Validate(xmlOptions);
                isXmlConfigured = true;
                LogString += "config.xml is loaded.\n";
            }
            catch (Exception ex)
            {
                isXmlConfigured = false;
                LogString += ex.Message;
            }

            if (!isJsonConfigured && !isXmlConfigured)
            {                
                LogString += "default configuration.\n";
            }
        }

        object FindOption<T>(object options)
        {
            if (typeof(T) == defaultOptions.GetType())
            {
                return options;
            }

            try
            {
                return options.GetType().GetProperty(typeof(T).Name).GetValue(options, null);
            }
            catch
            {             
                throw new NotImplementedException();
            }
        }

        public object GetOptions<T>()
        {
            if (isJsonConfigured)
            {                
                return FindOption<T>(jsonOptions);
            }
            else if (isXmlConfigured)
            {              
                return FindOption<T>(xmlOptions);
            }
            else
            {               
                return FindOption<T>(defaultOptions);
            }
        }
    }
}
