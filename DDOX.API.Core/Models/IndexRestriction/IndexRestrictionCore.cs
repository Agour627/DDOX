using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.IndexRestriction
{
    public class IndexRestrictionCore
    {
        public int Id { get; set; }
        public int IndexId { get; set; }
        public int RestrictionId { get; set; }
        public string Value { get; set; }
        public string SecondValueOption { get; set; }
    }
}
