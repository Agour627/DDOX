using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Dtos.IndexRestection
{
    public class IndexRestrictionToAddDto
    {
        public int RestrictionId { get; set; }
        public string Value { get; set; }
        public string SecondValueOption { get; set; }
    }
}
