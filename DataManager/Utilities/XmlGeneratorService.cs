using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DataAccessLayer.Options;
using DataAccessLayer.Models;

namespace DataManager.Utilities
{
    public class XmlGeneratorService
    {
        SendingOptions options;

        public XmlGeneratorService(SendingOptions options)
        {
            this.options = options;
            Directory.CreateDirectory(this.options.SourceDir);
        }

        public void SerializeXML(PersonData purchasingInfo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PersonData));
            using (FileStream fs = new FileStream(Path.Combine(options.SourceDir,"personData.xml"), FileMode.Create))
            {
                serializer.Serialize(fs, purchasingInfo);
            }
        }
    }
}
