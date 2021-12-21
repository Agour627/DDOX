using DDOX.API.Dtos.CategoryIndices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Category
{
    public class CategoryToUpdateDto
    {
        public string Name { get; set; }
        public int UpdatedBy { get; set; }
        public List<CategoryIndicesDto> CategoryIndices { get; set; }
    }
}
