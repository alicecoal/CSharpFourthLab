using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptionsManager;
using DataAccessLayer.Options;

namespace DataAccessLayer.Utilities
{
    public class Validator : IValidator
    {
        public bool MakeValidDir(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public bool MakeValidFile(string path)
        {
            if (!File.Exists(path))
            {
                if (!MakeValidDir(Path.GetDirectoryName(path)))
                {
                    return false;
                }

                try
                {
                    File.Create(path).Close();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public void Validate(object obj)
        {
            DataAccessOptions options = obj as DataAccessOptions;

            if (!MakeValidDir(options.SendingOptions.SourceDir))
            {
                options.SendingOptions.SourceDir = @"F:\fourth\SDir";
                MakeValidDir(options.SendingOptions.SourceDir);

                throw new UnauthorizedAccessException("No access to source directory -> default directory.");
            }
        }
    }
}