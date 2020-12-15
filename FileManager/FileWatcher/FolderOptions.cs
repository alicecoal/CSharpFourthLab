using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class FolderOptions : Options
    {
        public string SourceDir { get; set; } = @"F:\fourth\FileManager\SDir";
        public string TargetDir { get; set; } = @"F:\fourth\FileManager\TDir";

        public FolderOptions()
        { }
    }
}
