using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
   public class FolderCategories
    {
        public int Id { get; set; }
        [ForeignKey("Folder")]
        public int FolderId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Folder Folder { get; set; }
        public Category Category { get; set; }

    }
}
