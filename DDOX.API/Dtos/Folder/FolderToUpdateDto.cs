using DDOX.API.Dtos.FolderCategories;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Folder
{
    public class FolderToUpdateDto
    {
        public string Name { get; set; }
        public int UpdatedBy { get; set; }
        public List<FolderIndicesDto> FolderIndices { get; set; }
        public List<FolderCategoriesDto> FolderCategories { get; set; }
    }
}
