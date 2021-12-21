using DDOX.API.Dtos.File;
using DDOX.API.Dtos.FolderCategories;
using DDOX.API.Dtos.FolderIndices;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Folder
{
    public class FolderToDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FolderType FolderType { get; set; }
        public bool IsActive { get; set; }
        public List<FolderToDisplayDto> SubFolders { get; set; }
        public List<FileToDisplayDto> Files { get; set; }
        public List<FolderIndicesDto> FolderIndices { get; set; }
        public List<FolderCategoriesDto> FolderCategories { get; set; }
    }
}
