using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.Partiton
{
    public class PartitionToCreateDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int CreatedBy { get; set; }

    }
}
