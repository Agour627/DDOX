using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.Partition
{
    public class PartitionCore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEncrypted { get; set; }
        public string Path { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }
}
