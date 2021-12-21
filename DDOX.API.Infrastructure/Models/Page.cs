using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Page
    {
        public int Id { get; set; }
        public int PageNumber { get; set; }
        [ForeignKey("File")]
        public int FileId { get; set; }
        public File File { get; set; }
        public List<PageIndices> PageIndices { get; set; }

    }
}
