using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.File
{
    public class FileToMoveDto
    {
        public int FileId { get; set; }
        public int DestinationId { get; set; }
        public int UpdatedBy { get; set; }

    }
}
