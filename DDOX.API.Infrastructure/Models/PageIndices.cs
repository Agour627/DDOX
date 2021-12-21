using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class PageIndices
    {
        public int Id { get; set; }
        [ForeignKey("Index")]
        public int IndexId { get; set; }
        [ForeignKey("Page")]
        public int PageId { get; set; }
        public string Value { get; set; }
        public Index Index { get; set; }
        public Page Page { get; set; }

    }
}
