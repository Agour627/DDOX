using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Category
{
    public class CategoryCore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartitionId { get; set; }
        public EntityDefinition Type { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public List<CategoryIndicesCore> CategoryIndices { get; set; }

    }
}
