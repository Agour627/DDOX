using DDOX.API.Core.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Page
{
    public class PageCore
    {
        public int Id { get; set; }
        public int PageNumber { get; set; }
        public int FileId { get; set; }
        public List<PageIndicesCore> PageIndices { get; set; }
    }
}
