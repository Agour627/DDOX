using DDOX.API.Dtos.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Partition
{
    public class PartitionToDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<FolderToDisplayDto> Folders { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
