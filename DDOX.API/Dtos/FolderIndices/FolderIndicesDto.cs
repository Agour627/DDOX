using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Folder
{
    public class FolderIndicesDto : IEntityIndices
    {
        public int? Id { get; set; }
        public int IndexId { get; set; }
        public string Value { get; set; }

    }
}
