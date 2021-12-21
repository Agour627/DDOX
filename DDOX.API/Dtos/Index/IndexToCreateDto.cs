using DDOX.API.Dtos.IndexRestection;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Index
{
    public class IndexToCreateDto
    {
        public string Label { get; set; }
        public string DefaultValue { get; set; }
        public int DataType { get; set; }
        public EntityDefinition Type { get; set; }
        public int PartitionId { get; set; }
        public int CreatedBy { get; set; }
        public List<IndexRestrictionToAddDto> IndexRestrictions { get; set; }
    }
}
