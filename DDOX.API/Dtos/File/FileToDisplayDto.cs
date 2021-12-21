using DDOX.API.Dtos.FileIndicesCore;
using DDOX.API.Dtos.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.File
{
    public class FileToDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AliasName { get; set; }
        public long FileSize { get; set; }
        public int FolderId { get; set; }
        public bool IsActive { get; set; }
        public bool IsEncrypted { get; set; }
        public int PartitionId { get; set; }
        public string FullPath { get; set; }
        public List<FileIndicesDto> FileIndices { get; set; }
        public List<PageDto> Pages { get; set; }
    }
}
