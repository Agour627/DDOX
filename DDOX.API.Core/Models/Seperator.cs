using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models
{
    public class Seperator
    {
        public SeperatorType SeperatorType { get; set; }
        public int? PagesNumber { get; set; }
        public int Level { get; set; }
        public string FolderName { get; set; }
        public bool ScanSeperator { get; set; } = false;
    }
}
