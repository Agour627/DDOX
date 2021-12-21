using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Extension
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public bool IsAllowed { get; set; } 

    }
}
