using DDOX.API.Core.Models;
using DDOX.API.Dtos.Category;
using DDOX.API.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.File
{
    public class FileToCreateDto
    {
        public UploadType UploadType { get; set; }
        public int PartitionId { get; set; }
        public int FolderId { get; set; }
        public int CreatedBy { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<Seperator> Seperators { get; set; }

    }
}
