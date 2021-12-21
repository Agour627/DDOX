using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Models.FileIndices
{
    public class FileIndicesCore
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int IndexId { get; set; }
        public string Value { get; set; }
    }
}
