using DDOX.API.Core.Models.File;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IFileService
    {
        Task<bool> AddFile(FileCore fileCore);
        Task<bool> UpdateFile(int id, FileCore fileCore);
        Task<List<FileCore>> GetFiles(int folderId);
        Task<FileCore> GetFileById(int id);
        bool MoveFile(string sourcePath, string destinationPath, string fileName);
        Task<Stream> Save(IFormFile file, string path, bool isEncrypted);
        Task<MemoryStream> Get(string path, bool isEncrypted);
    }
}
