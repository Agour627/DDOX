using DDOX.API.Core.Models.File;
using DDOX.API.Core.Models.FolderCategory;
using DDOX.API.Core.Models.FolderIndices;
using DDOX.API.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Folder
{
    public class FolderCore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FolderType FolderType { get; set; }
        public int? ParentId { get; set; }
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public int SeperatorLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<FolderCore> SubFolders { get; set; } = new List<FolderCore>();
        public List<FileCore> Files { get; set; }
        public List<IFormFile> SubFiles { get; set; } 
        public List<FolderIndicesCore> FolderIndices { get; set; }
        public List<FolderCategoriesCore> FolderCategories { get; set; }
    }
}
