using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Category
{
    public class CategoryToCreateDto
    {
        public string Name { get; set; }
        public int PartitionId { get; set; }
        public EntityDefinition Type { get; set; }
        public int CreatedBy { get; set; }
        public List<CategoryIndicesToAddDto> CategoryIndices { get; set; }
    }
}
