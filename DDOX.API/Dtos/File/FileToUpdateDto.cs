using DDOX.API.Dtos.FileIndicesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.File
{
    public class FileToUpdateDto
    {
        public string Name { get; set; }
        public int UpdatedBy { get; set; }
        public List<FileIndicesDto> FileIndices { get; set; }
    }
}
