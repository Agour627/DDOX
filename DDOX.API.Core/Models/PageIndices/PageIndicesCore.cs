using DDOX.API.Core.Models.Index;
using DDOX.API.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models
{
    public class PageIndicesCore
    {
        public int Id { get; set; }
        public int IndexId { get; set; }
        public int PageId { get; set; }
        public string Value { get; set; }
    }
}
