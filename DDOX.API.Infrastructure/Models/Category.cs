using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Category : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Partition")]
        public int PartitionId { get; set; }
        public EntityDefinition Type { get; set; }
        public bool IsActive { get; set; }

        public Partition Partition { get; set; }
        public List<CategoryIndices> CategoryIndices { get; set; }
        public List<FolderCategories> FolderCategories { get; set; }

    }
}
