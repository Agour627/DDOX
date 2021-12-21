using DDOX.API.Core.Models.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IFolderService
    {
        Task<FolderCore> CreateFolder(FolderCore folderCore);
        Task<bool> UpdateFolder(int id, FolderCore folderCore);
        Task<List<FolderCore>> GetFoldersInPartition(int partitionId);
        Task<FolderCore> GetFolderById(int id);
        Task<string> GetFolderPath(int folderId, string partitionPath);
        bool MoveFolder(string sourcePath, string destinationPath);
        bool SaveFolder(string folderFullPath);
        Task<FolderCore> GetFolder(int id);

    }
}
