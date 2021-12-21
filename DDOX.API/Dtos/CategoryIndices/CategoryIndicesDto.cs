using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.CategoryIndices
{
    public class CategoryIndicesDto
    {
        public int? Id { get; set; }
        public int IndexId { get; set; }
        public bool IsRequried { get; set; }
    }
}
