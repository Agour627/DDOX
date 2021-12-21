using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class FolderIndices
    {
        public int Id { get; set; }
        [ForeignKey("Folder")]
        public int FolderId { get; set; }
        [ForeignKey("Index")]
        public int IndexId { get; set; }
        public string Value { get; set; }

        public Folder Folder { get; set; }
        public Index Index { get; set; }

    }
}
