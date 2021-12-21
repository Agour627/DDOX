using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Folder
{
    public class FolderToMoveDto
    {
        public int FolderId { get; set; }
        public int? DestinationId { get; set; }
        public int UpdatedBy { get; set; }

    }
}
