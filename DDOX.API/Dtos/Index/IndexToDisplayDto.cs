using DDOX.API.Dtos.IndexRestection;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Index
{
    public class IndexToDisplayDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string DefaultValue { get; set; }
        public string DataType { get; set; }
        public EntityDefinition Type { get; set; }
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateddAt { get; set; }
        public List<IndexRestrictionDto> IndexRestrictions { get; set; }

    }
}
