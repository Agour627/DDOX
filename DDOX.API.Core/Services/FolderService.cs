using AutoMapper;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class FolderService : IFolderService
    {

        private readonly IGenericRepository<Folder> _folderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public FolderService(
                            IGenericRepository<Folder> folderRepository,
                            IMapper mapper,
                            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _folderRepository = folderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FolderCore> CreateFolder(FolderCore folderCore)
        {

            var folderToAdd = _mapper.Map<Folder>(folderCore);
            var addedFolder = await _folderRepository.Add(folderToAdd);
            var isAdded = await _unitOfWork.Save();
            if (!isAdded)
                return null;

            Log.ForContext("Info", new { EntityId = addedFolder.Id, PartitionId = addedFolder.PartitionId }, true)
               .Information($"User : {addedFolder.CreatedBy} Successfully Added Folder {addedFolder.Id} ");
            return _mapper.Map<FolderCore>(addedFolder);
        }

        public async Task<FolderCore> GetFolderById(int id)
        {
            var fetchedFolder = await _folderRepository.FindByCondition(folder => folder.Id == id,
                                                                        folder => folder.Include(folder => folder.SubFolders)
                                                                                        .Include(folder => folder.Files)
                                                                                        .Include(folder => folder.FolderCategories)
                                                                                        .Include(folder => folder.FolderIndices));
            if (fetchedFolder.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"Database Doesn't Contain Folder : {id}");
                return null;
            }
            return _mapper.Map<FolderCore>(fetchedFolder.FirstOrDefault());
        }

        public bool SaveFolder(string folderFullPath)
        {
            if (Directory.Exists(folderFullPath))
            {
                Log.Information($"This Folder Path : {folderFullPath} , Is Already Exists");
                return false;
            }
            try
            {
                Directory.CreateDirectory(folderFullPath);
                Log.ForContext("Info", new { FolderPath = folderFullPath }, true)
                   .Information($"Folder with Path : {folderFullPath} Successfully Saved ");
                return true;
            }
            catch (Exception ex)
            {
                Log.ForContext("Data", new { FolderPath = folderFullPath }, true)
                    .Warning(ex, ex.Message);
                return false;
            }

        }

        public async Task<List<FolderCore>> GetFoldersInPartition(int partitionId)
        {
            var fetchedFolders = await _folderRepository.FindByCondition(f => f.PartitionId == partitionId && f.IsActive,
                                                                          f => f.Include(folder => folder.FolderIndices)
                                                                                .Include(folder => folder.FolderCategories)
                                                                                .Include(folder => folder.Files.Where(file => file.IsActive)));
            if (fetchedFolders.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = partitionId }, true)
                   .Information($"Partition: {partitionId} Doesn't Contain Any Folders ");
                return null;
            }

            var sortedFolders = SortFolders(fetchedFolders, null, new List<Folder>());
            return _mapper.Map<List<FolderCore>>(sortedFolders);
        }

        public bool MoveFolder(string sourcePath, string destinationPath)
        {
            //Check If Destination path exists
            if (Directory.Exists(destinationPath))
            {
                Log.ForContext("Info", new { DestinationPath = destinationPath }, true)
                   .Information($"This Folder Path : {destinationPath} , Is Already Exists");
                return false;
            }
            try
            {
                Directory.Move(sourcePath, destinationPath);
                Log.ForContext("Info", new { SourcePath = sourcePath, DestinationPath = destinationPath }, true)
                   .Information($"Successfully Moved Folder From: {sourcePath} To: {destinationPath}");
            }
            catch (Exception ex)
            {
                Log.ForContext("Data", new { SourcePath = sourcePath, DestinationPath = destinationPath }, true)
                   .Warning(ex, ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateFolder(int id, FolderCore folderCore)
        {
            var oldFolder = await _folderRepository.FindByCondition(_folder => _folder.Id == id,
                                                                   relation => relation.Include(_folder => _folder.FolderIndices)
                                                                                       .Include(_folder => _folder.FolderCategories));

            var folderToUpdate = _mapper.Map(folderCore, oldFolder.FirstOrDefault());

            await _folderRepository.Update(folderToUpdate);
            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"User : {folderCore.UpdatedBy} Successfully Updated Folder : {id}");

            return isUpdated;
        }

        public async Task<string> GetFolderPath(int folderId, string partitionPath)
        {
            var breaker = CheckOperatingSystem();
            var fetchedFolder = await _folderRepository.GetById(folderId);
            var folderFullPath = breaker;
            while (fetchedFolder != null)
            {
                folderFullPath = breaker + fetchedFolder.Name + folderFullPath;
                if (fetchedFolder.ParentId != null)
                    fetchedFolder = await _folderRepository.GetById(fetchedFolder.ParentId ?? 0);
                else
                    fetchedFolder = null;
            }
            return partitionPath + folderFullPath;
        }

        private List<Folder> SortFolders(List<Folder> fetchedFolders, int? folderId, List<Folder> sortedFolders)
        {
            sortedFolders = fetchedFolders.Where(f => f.ParentId == folderId).ToList();
            foreach (var folder in sortedFolders)
            {
                folder.SubFolders = SortFolders(fetchedFolders, folder.Id, sortedFolders);
            }
            return sortedFolders;
        }
        private string CheckOperatingSystem()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";
        }

        public async Task<FolderCore> GetFolder(int id)
        {
            var fetchedFolder = await _folderRepository.GetById(id);
            var foldercore = _mapper.Map<FolderCore>(fetchedFolder);
            return foldercore;
        }
    }
}
