using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos
{
    public interface IEntityIndices
    {
        public int IndexId { get; set; }
        public string Value { get; set; }
    }
}
