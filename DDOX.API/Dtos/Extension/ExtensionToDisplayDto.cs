using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Extension
{
    public class ExtensionToDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public bool IsAllowed { get; set; }
    }
}
