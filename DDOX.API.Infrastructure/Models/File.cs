using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class File : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AliasName { get; set; }
        public long FileSize { get; set; }
        [ForeignKey("Folder")]
        public int FolderId { get; set; }
        public bool IsEncrypted { get; set; }
        [ForeignKey("Partition")]
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public Partition Partition { get; set; }
        public Folder Folder { get; set; }
        public List<FileIndices> FileIndices { get; set; }
        public List<Page> Pages { get; set; }


    }
}
