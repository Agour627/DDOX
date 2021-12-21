using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Restriction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IndexRestrictions> IndexRestrictions { get; set; }

    }
}
