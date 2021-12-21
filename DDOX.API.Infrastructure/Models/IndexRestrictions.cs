using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class IndexRestrictions
    {
        public int Id { get; set; }

        [ForeignKey("Index")]
        public int IndexId { get; set; }
        [ForeignKey("Restriction")]
        public int RestrictionId { get; set; }
        public string Value { get; set; }
        public string SecondValueOption { get; set; }
        public Index Index { get; set; }
        public Restriction Restriction { get; set; }
    }
}
