using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Folder : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FolderType  FolderType { get; set; }

        [ForeignKey("ParentFolder")]
        public int? ParentId { get; set; }

        [ForeignKey("Partition")]
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public Folder ParentFolder { get; set; }
        public Partition Partition { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<File> Files { get; set; }
        public List<FolderIndices> FolderIndices { get; set; }
        public List<FolderCategories> FolderCategories { get; set; }
    
}
}
