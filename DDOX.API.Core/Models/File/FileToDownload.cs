using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.File
{
    public class FileToDownload
    {
        public MemoryStream Stream { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }
}
