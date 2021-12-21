using DDOX.API.Core.Models.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IExtensionService
    {
        Task<List<ExtensionCore>> GetAllExtentions();
        Task<ExtensionCore> GetExtensionById(int id);
        Task<ExtensionCore> GetExtensionByFileName(string fileName);
        Task<bool> UpdateExtension(int id, ExtensionCore extensionCore);
    }
}
