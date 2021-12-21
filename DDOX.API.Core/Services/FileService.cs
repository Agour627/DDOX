using AutoMapper;
using DDOX.API.Core.Models.File;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Configurations;
using File = DDOX.API.Infrastructure.Models.File;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DDOX.API.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IGenericRepository<File> _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly EncryptSettings _encryptSettings;

        public FileService(IGenericRepository<File> fileRepository,
                            IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IOptions<EncryptSettings> options
                           )
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _encryptSettings = options.Value;
        }
        public async Task<bool> AddFile(FileCore fileCore)
        {
            var fileToAdd = _mapper.Map<File>(fileCore);
            var addedFile = await _fileRepository.Add(fileToAdd);
            var isAdded = await _unitOfWork.Save();
            if (isAdded)
                Log.ForContext("Info", new { EntityId = addedFile.Id, PartitionId = addedFile.PartitionId }, true)
                   .Information($"User: {addedFile.CreatedBy} successfully added file: {addedFile.Id}");
            return isAdded;
        }

        public async Task<FileCore> GetFileById(int id)
        {
            var fetchedFile = await _fileRepository.FindByCondition(file => file.Id == id,
                                                                    file => file.Include(file => file.FileIndices)
                                                                                 .Include(file => file.Pages)
                                                                                 .ThenInclude(page => page.PageIndices));
            if (fetchedFile.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"Data Base doesn't contain file: {id}");
                return null;
            }
            return _mapper.Map<FileCore>(fetchedFile.FirstOrDefault());
        }

        public async Task<List<FileCore>> GetFiles(int folderId)
        {
            var fetchedFiles = await _fileRepository.FindByCondition(file => file.FolderId == folderId);
            if (fetchedFiles.Count == 0)
            {
                Log.Information($"Folder: {folderId} doesn't contain files");
                return null;
            }
            return _mapper.Map<List<FileCore>>(fetchedFiles);
        }
      
        public async Task<bool> UpdateFile(int id, FileCore fileCore)
        {
            var oldFile = await _fileRepository.FindByCondition(_file => _file.Id == id,
                                                                relation => relation.Include(_file => _file.FileIndices));
            
            var fileToUpdate = _mapper.Map(fileCore, oldFile.FirstOrDefault());
            await _fileRepository.Update(fileToUpdate);
            
            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new { EntityId = id }, true)
                  .Information($"User: {fileToUpdate.UpdatedBy} successfully updated file: {id}");

            return isUpdated;
        }

        public bool MoveFile(string sourcePath, string destinationPath, string fileName)
        {
            var sourceFullPath = Path.Combine(sourcePath, fileName);
            var destinationFullPath = Path.Combine(destinationPath, fileName);
            try
            {
                Directory.Move(sourceFullPath, destinationFullPath);
            }
            catch (Exception ex)
            {
                Log.ForContext("Data", new { SourcePath = sourcePath, DestinationPath = destinationPath, FileName = fileName }, true)
                   .Warning(ex, ex.Message);
                return false;
            }

            Log.ForContext("Info", new { SourcePath = sourcePath, DestinationPath = destinationPath, FileName = fileName }, true)
               .Information($"File: {fileName} have been moved Successfully from: {sourcePath} ,to: {destinationPath}");
            return true;
        }

        public async Task<Stream> Save(IFormFile file, string path, bool isEncrypted)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                if (isEncrypted)
                {
                    byte[] key = new UnicodeEncoding().GetBytes(_encryptSettings.EncryptionKey);
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream,
                                                                        new RijndaelManaged().CreateEncryptor(key, key),
                                                                        CryptoStreamMode.Write))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            await cryptoStream.WriteAsync(memoryStream.ToArray());
                            Log.ForContext("Data", new { Path = path }, true)
                               .Information($"File: {file.FileName} have been saved successfully ");
                            return memoryStream;
                        }
                    }
                }
                else
                {
                    await file.CopyToAsync(fileStream);
                }
                Log.ForContext("Data", new { Path = path }, true)
                   .Information($"File: {file.FileName} have been saved successfully ");
                return fileStream;
            }
            catch (Exception ex)
            {
                Log.ForContext("Data", new { Path = path }, true)
                   .Warning(ex, ex.Message);
                return null;
            }
        }
       
        public async Task<MemoryStream> Get(string path, bool isEncrypted)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    var memoryStream = new MemoryStream();
                    if (isEncrypted)
                    {
                        byte[] key = new UnicodeEncoding().GetBytes(_encryptSettings.EncryptionKey);
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream,
                                                                            new RijndaelManaged().CreateDecryptor(key, key),
                                                                            CryptoStreamMode.Read))
                        {
                            await cryptoStream.CopyToAsync(memoryStream);
                        }
                    }
                    else
                    {
                        await fileStream.CopyToAsync(memoryStream);
                    }
                    return memoryStream;

                }
            }
            catch (Exception ex)
            {
                Log.ForContext("Data", new { Path = path }, true)
                   .Warning(ex, ex.Message);
                return null;
            }
        }


    }
}
