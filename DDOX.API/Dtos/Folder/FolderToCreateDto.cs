using DDOX.API.Dtos.FolderIndices;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Folder
{
    public class FolderToCreateDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int PartitionId { get; set; }
        public FolderType FolderType { get; set; }
        public int CreatedBy { get; set; }
        public List<FolderIndicesToCreateDto> FolderIndices { get; set; }
        public List<FolderCategoriesToAddDto> FolderCategories { get; set; }
    }
}
