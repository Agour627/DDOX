using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Extension
{
    public class ExtensionCore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public bool IsAllowed { get; set; }
    }
}
