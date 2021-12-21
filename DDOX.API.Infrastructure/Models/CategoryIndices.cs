using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class CategoryIndices
    {
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Index")]
        public int IndexId { get; set; }
        public bool IsRequried { get; set; }

        public Index Index { get; set; }
        public Category Category { get; set; }
    }
}
