using DDOX.API.Dtos.IndexRestection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Index
{
    public class IndexToUpdateDto
    {
        public string Label { get; set; }
        public int UpdatedBy { get; set; }
        public string DefaultValue { get; set; }
        public List<IndexRestrictionDto> IndexRestrictions { get; set; }
    }
}
