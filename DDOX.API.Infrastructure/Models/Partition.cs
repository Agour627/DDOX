using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Infrastructure.Models
{
    public class Partition : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEncrypted { get; set; } = true;
        public string Path { get; set; }

        public List<Index> Indices { get; set; }
        public List<Category> Categories { get; set; }
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }

    }
}
