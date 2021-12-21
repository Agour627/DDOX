using DDOX.API.Dtos.CategoryIndices;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Category
{
    public class CategoryToDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartitionId { get; set; }
        public EntityDefinition Type { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
        public List<CategoryIndicesDto> CategoryIndices { get; set; }

    }
}
