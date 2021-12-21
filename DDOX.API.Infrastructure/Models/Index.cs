using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Index : BaseModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string DefaultValue { get; set; }
        public DataType DataType { get; set; }
        public EntityDefinition Type { get; set; }

        [ForeignKey("Partition")]
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }

        public Partition Partition { get; set; }
        public List<IndexRestrictions> IndexRestrictions { get; set; }
        public List<CategoryIndices> CategoryIndices { get; set; }
        public List<FolderCategories> FolderIndices { get; set; }
        public List<FileIndices> FileIndices { get; set; }
        public List<PageIndices> PageIndices { get; set; }

    }
}
