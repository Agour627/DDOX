using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
   public class FileIndices
    {
        public int Id { get; set; }
        [ForeignKey("File")]
        public int FileId { get; set; }
        [ForeignKey("Index")]
        public int IndexId { get; set; }
        public string Value { get; set; }

        public File File { get; set; }
        public Index Index { get; set; }
    }
}
