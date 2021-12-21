using AutoMapper;
using DDOX.API.Core.Helpers;
using DDOX.API.Core.Models;
using DDOX.API.Core.Models.File;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.File;
using DDOX.API.Infrastructure.Enums;
using DDOX.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFolderService _folderService;
        private readonly IPartitionSevice _partitionSevice;
        private readonly IExtensionService _extentionService;
        private readonly IMapper _mapper;

        public FileController(
                            IFileService fileService,
                            IFolderService folderService,
                            IPartitionSevice partitionSevice,
                            IExtensionService extentionService,
                            IMapper mapper
                            )
        {
            _fileService = fileService;
            _folderService = folderService;
            _partitionSevice = partitionSevice;
            _extentionService = extentionService;
            _mapper = mapper;
        }

        [HttpPost("Seperator")]
        public IActionResult GenerateBarcode()
        {
            Log.Information("User try to create new barcode");
            var file = SeperatorSettings.GenerateBarcode();
            file.Stream.Position = 0;
            return File(file.Stream, file.MimeType, file.FileName);
        }


        [ServiceFilter(typeof(ExtensionsValidator))]
        [HttpPost]
        public async Task<IActionResult> AddFiles([FromForm] FileToCreateDto fileToCreateDto)
        {
            Log.ForContext("Info", new { PartitionId = fileToCreateDto.PartitionId }, true)
               .Information($"User: {fileToCreateDto.CreatedBy} tried to add new files in Partition {fileToCreateDto.PartitionId}");

            var fetchedPartition = await _partitionSevice.GetPartitionById(fileToCreateDto.PartitionId);
            if (fetchedPartition is null)
                return BadRequest("Wrong Partition");

            var fetchedParentFolder = await _folderService.GetFolder(fileToCreateDto.FolderId);
            if (fetchedParentFolder is null)
                return BadRequest("This folder doesn't exist");

            var folderPath = await _folderService.GetFolderPath(fileToCreateDto.FolderId, fetchedPartition.Path);
            var fileToAddCore = _mapper.Map<FileCore>(fileToCreateDto, opt =>
              {
                  opt.AfterMap((s, d) =>
                  {
                      d.IsEncrypted = fetchedPartition.IsEncrypted;
                      d.FolderPath = folderPath;
                  });
              });

            List<IFormFile> seperatedFiles = new();
            List<FolderCore> seperatedFolders = new();
            if (fileToCreateDto.UploadType == UploadType.Seperator)
            {
                //List<FolderCore> seperatedFolders = SeperateFilesInFolders(fileToCreateDto.Files, fileToCreateDto.Seperators);

                foreach (var _file in fileToCreateDto.Files)
                {
                    var isSeperator = CheckSeperator(fileToCreateDto.Seperators, _file, seperatedFiles.Count);
                    if (isSeperator is null)
                        seperatedFiles.Add(_file);

                    else
                    {
                        var initiatedFolder = new FolderCore()
                        {
                            SubFiles = new List<IFormFile>(seperatedFiles),
                            SeperatorLevel = isSeperator.Level,
                            //Add Function to generate Folder name.
                            Name = isSeperator.FolderName ?? seperatedFiles.FirstOrDefault().FileName

                        };
                        seperatedFolders = ReArrangeFolders(seperatedFolders, initiatedFolder);
                        seperatedFiles.Clear();
                    }
                }
                if (seperatedFiles.Count > 0)
                {
                    var initiatedFolder = new FolderCore()
                    {
                        SubFiles = seperatedFiles,
                        Name = seperatedFiles.FirstOrDefault().FileName
                    };
                    seperatedFolders.Add(initiatedFolder);
                }

                var isPopulated = await PopulateFoldersAndFiles(seperatedFolders, fileToAddCore, fetchedParentFolder.Id);

                if (!isPopulated)
                    return BadRequest("Couldn't Upload All Files");
            }
            else
            {
                var isUploaded = await UploadFiles(fileToAddCore, fileToCreateDto.Files);
                if (!isUploaded)
                    return BadRequest("Couldn't Upload All Files");

            }

            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileData([FromRoute] int id)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User try to fetch file: {id}");

            var fetchedFile = await _fileService.GetFileById(id);
            if (fetchedFile == null)
                return NotFound();

            var fileToDisplay = _mapper.Map<FileToDisplayDto>(fetchedFile);
            return Ok(fileToDisplay);
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile([FromRoute] int id)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User tried to download file: {id}");

            var fetchedFile = await _fileService.GetFileById(id);
            if (fetchedFile is null)
                return NotFound();

            var fetchedRepository = await _partitionSevice.GetPartitionById(fetchedFile.PartitionId);
            if (fetchedRepository == null)
                return NotFound("Wrong Repository");

            var folderPath = await _folderService.GetFolderPath(fetchedFile.FolderId, fetchedRepository.Path);
            var fileFullPath = Path.Combine(folderPath, fetchedFile.AliasName);

            var memoryStream = await _fileService.Get(fileFullPath, fetchedFile.IsEncrypted);
            memoryStream.Position = 0;

            var fetchedFileExtension = await _extentionService.GetExtensionByFileName(fetchedFile.Name);
            if (fetchedFileExtension is null)
                return NotFound("The Extension doesn't exist");

            return File(memoryStream, fetchedFileExtension.MimeType, fetchedFile.Name);
        }

        [HttpPut("Move")]
        public async Task<IActionResult> MoveFile([FromBody] FileToMoveDto fileToMoveDto)
        {
            Log.ForContext("Info", new { EntityId = fileToMoveDto.FileId}, true)
               .Information($"User: {fileToMoveDto.UpdatedBy} tried to move file: {fileToMoveDto.FileId}");

            var fetchedFile = await _fileService.GetFileById(fileToMoveDto.FileId);
            if (fetchedFile is null)
                return NotFound();

            //Get Source And Destination Folder  And Check its Existance
            var fetchedSource = await _folderService.GetFolder(fetchedFile.FolderId);
            var fetchedDestination = await _folderService.GetFolder(fileToMoveDto.DestinationId);

            if (fetchedSource is null || fetchedDestination is null)
                return NotFound("Source or Destination Is Not Found");

            if (fetchedDestination.FolderType == FolderType.Folder)
            {
                Log.ForContext("Info", new { EntityId = fileToMoveDto.FileId, DestinationId = fileToMoveDto.DestinationId }, true)
                   .Information($"User : {fileToMoveDto.UpdatedBy} try to move file : {fileToMoveDto.FileId} to Folder : {fileToMoveDto.DestinationId}");
                return BadRequest("can not move files to direct Folder");
            }


            var fetchedRepository = await _partitionSevice.GetPartitionById(fetchedFile.PartitionId);
            if (fetchedRepository is null)
                return NotFound("The partition is not Found");

            string sourcePath = await _folderService.GetFolderPath(fetchedFile.FolderId, fetchedRepository.Path);
            string destinationPath = await _folderService.GetFolderPath(fileToMoveDto.DestinationId, fetchedRepository.Path);

            var isMoved = _fileService.MoveFile(sourcePath, destinationPath, fetchedFile.AliasName);
            if (!isMoved)
                return BadRequest("Couldn't Move File");

            fetchedFile.FolderId = fileToMoveDto.DestinationId;
            fetchedFile.UpdatedBy = fileToMoveDto.UpdatedBy;
            var isUpdated = await _fileService.UpdateFile(fileToMoveDto.FileId, fetchedFile);
            if (!isUpdated)
                return BadRequest("Couldn't Update File");

            return Ok(isUpdated);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFile([FromRoute] int id, [FromBody] FileToUpdateDto fileToUpdateDto)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
              .Information($"User: {fileToUpdateDto.UpdatedBy} try to update file: {id}");

            var fetchedFile = await _fileService.GetFileById(id);
            if (fetchedFile is null)
                return NotFound("The File is not exist");

            if (fileToUpdateDto.Name != null)
            {
                var fetchedFileExtension = await _extentionService.GetExtensionByFileName(fetchedFile.Name);
                fileToUpdateDto.Name += fetchedFileExtension.Name;
            }
           
            if (fileToUpdateDto.FileIndices is not null)
            {
                var isValid = fileToUpdateDto.FileIndices.All(x => x.Id == null || fetchedFile.FileIndices.Any(f => f.Id == x.Id));
                if (!isValid)
                {
                    Log.ForContext("Info", new { EntityId = id }, true)
                     .Information($"FileIndex Id Is Not Valid When Update File :{id}");
                    return BadRequest("FileIndex Id Is Not Valid");
                }
            }

            var fileToUpdateCore = _mapper.Map(fileToUpdateDto, fetchedFile);
            var isUpdated = await _fileService.UpdateFile(id, fileToUpdateCore);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateFileStatus([FromRoute] int id, [FromBody] bool status)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
             .Information($"User tried to change the status of file: {id}");

            var fetchedFile = await _fileService.GetFileById(id);
            if (fetchedFile is null)
                return NotFound();

            fetchedFile.IsActive = status;
            fetchedFile.UpdatedAt = DateTime.Now;
            var isUpdated = await _fileService.UpdateFile(id, fetchedFile);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }


        private static string CreateAliasName(string fileName)
        {
            var extention = Path.GetExtension(fileName);
            var removeExtention = fileName.Replace(extention, "");
            var aliasName = $"{removeExtention}-{DateTime.Now.Ticks}{extention}";
            return aliasName;
        }

        private async Task<bool> UploadFiles(FileCore fileCore, List<IFormFile> files)
        {
            foreach (var _file in files)
            {
                fileCore.Name = _file.FileName;
                fileCore.FileSize = _file.Length;
                fileCore.AliasName = CreateAliasName(_file.FileName);
                var fileFullPath = Path.Combine(fileCore.FolderPath, fileCore.AliasName);
                var isSaved = await _fileService.Save(_file, fileFullPath, fileCore.IsEncrypted);
                if (isSaved is null)
                    return false;
                var isAdded = await _fileService.AddFile(fileCore);
                if (!isAdded)
                    return false;
            }
            return true;
        }

        private List<FolderCore> ReArrangeFolders(List<FolderCore> folders, FolderCore currentFolder)
        {
            if (folders.Any(x => x.SeperatorLevel > currentFolder.SeperatorLevel))
            {
                foreach (var folder in folders)
                {
                    if (folder.SeperatorLevel > currentFolder.SeperatorLevel)
                    {
                        currentFolder.SubFolders.Add(folder);
                    }
                }
                folders = folders.Except(currentFolder.SubFolders).ToList();
            }
            folders.Add(currentFolder);
            return folders;
        }
        private Seperator CheckSeperator(List<Seperator> seperators, IFormFile file, int filesCount)
        {
            foreach (var seperator in seperators)
            {
                if (SeperatorSettings.IsSeperator(file, seperator, filesCount))
                    return seperator;
            }
            return null;
        }

        private async Task<bool> PopulateFoldersAndFiles(List<FolderCore> folders, FileCore fileCore, int parentId)
        {
            var parentFolderPath = fileCore.FolderPath;
            foreach (var folder in folders)
            {
                var folderToAddCore = _mapper.Map(fileCore, folder, opt =>
                {
                    opt.AfterMap((s, d) => d.ParentId = parentId);
                });
                var folderPath = Path.Combine(parentFolderPath, folder.Name);
                var isFolderDirectoryCreated = _folderService.SaveFolder(folderPath);
                if (!isFolderDirectoryCreated)
                    return false;

                var createdFolder = await _folderService.CreateFolder(folderToAddCore);
                if (createdFolder is null)
                    return false;

                fileCore.FolderPath = folderPath;
                fileCore.FolderId = createdFolder.Id;
                if (folder.SubFiles.Count != 0)
                {
                    await UploadFiles(fileCore, folder.SubFiles);
                }

                if (folder.SubFolders.Count != 0)
                    return await PopulateFoldersAndFiles(folder.SubFolders, fileCore, createdFolder.Id);
            }
            return true;
        }


        //private List<FolderCore> SeperateFilesInFolders(List<IFormFile> files, List<Seperator> seperators)
        //{
        //    List<IFormFile> seperatedFiles = new();
        //    List<FolderCore> seperatedFolders = new();
        //    foreach (var _file in files)
        //    {
        //        var isSeperator = CheckSeperator(seperators, _file, seperatedFiles.Count);
        //        if (isSeperator is null)
        //            seperatedFiles.Add(_file);

        //        else
        //        {
        //            var initiatedFolder = new FolderCore()
        //            {
        //                SubFiles=new List<IFormFile>(seperatedFiles),
        //                SeperatorLevel = isSeperator.Level,
        //                //Add Function to generate Folder name.
        //                Name = isSeperator.FolderName ?? seperatedFiles.FirstOrDefault().FileName

        //            };
        //            seperatedFolders = ReArrangeFolders(seperatedFolders, initiatedFolder);
        //            seperatedFiles.Clear();
        //        }
        //    }
        //    if(seperatedFiles.Count > 0)
        //    {
        //        var initiatedFolder = new FolderCore()
        //        {
        //            SubFiles = seperatedFiles,
        //            Name = seperatedFiles.FirstOrDefault().FileName
        //        };
        //        seperatedFolders.Add(initiatedFolder);
        //    }
        //    return seperatedFolders;
        //}





    }
}
