using DDOX.API.Core.Models.FileIndices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.File
{
    public class FileCore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AliasName { get; set; }
        public long FileSize { get; set; }
        public int FolderId { get; set; }
        public bool IsEncrypted { get; set; }
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public string FolderPath { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<FileIndicesCore> FileIndices { get; set; }

    }
}
