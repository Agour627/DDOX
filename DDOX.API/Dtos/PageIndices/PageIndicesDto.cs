using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.PageIndices
{
    public class PageIndicesDto : IEntityIndices
    {
        public int? Id { get; set; }
        public int IndexId { get; set; }
        public string Value { get; set; }
    }
}
