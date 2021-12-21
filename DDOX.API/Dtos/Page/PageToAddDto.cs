using DDOX.API.Dtos.PageIndices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Page
{
    public class PageToAddDto
    {
        public int PageNumber { get; set; }
        public int FileId { get; set; }
        public List<PageIndicesToAddDto> PageIndices { get; set; }

    }
}
