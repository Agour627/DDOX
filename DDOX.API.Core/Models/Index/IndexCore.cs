using DDOX.API.Core.Models.IndexRestriction;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Index
{
    public class IndexCore
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string DefaultValue { get; set; }
        public DataType DataType { get; set; }
        public EntityDefinition Type { get; set; }
        public int PartitionId { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public List<IndexRestrictionCore> IndexRestrictions { get; set; }

    }
}
