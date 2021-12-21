using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.FileIndices
{
    public class FileIndicesToAddDto : IEntityIndices
    {
        public int IndexId { get; set; }
        public string Value { get; set; }
    }
}
