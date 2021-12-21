using AutoMapper;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Models.FolderCategory;
using DDOX.API.Core.Models.FolderIndices;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Folder;
using DDOX.API.Dtos.FolderCategories;
using DDOX.API.Helpers;
using DDOX.API.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DDOX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IMapper _mapper;
        private readonly IPartitionSevice _partitionSevice;
        public FolderController(IFolderService folderService,
                               IMapper mapper,
                               IPartitionSevice partitionSevice
                             )
        {
            _folderService = folderService;
            _mapper = mapper;
            _partitionSevice = partitionSevice;
        }

        [HttpPost]
        public async Task<ActionResult> CreateFolder(FolderToCreateDto folderToCreateDto)
        {
            Log.ForContext("Info", new { PartitionId = folderToCreateDto.PartitionId }, true)
               .Information($"User : {folderToCreateDto.CreatedBy} Try To Add New Folder in partition: {folderToCreateDto.PartitionId}");

            // Validation whether the destination is a folder not partition
            // Folder's table has self relation to parent folder's id or null if the parent is a partition
            if (folderToCreateDto.ParentId is not null)
            {
                var parent = await _folderService.GetFolderById(folderToCreateDto.ParentId ?? 0);
                // Validate destination folder exist
                if (parent is null)
                    return NotFound($"folder Id :{folderToCreateDto.ParentId} is not exists");

                // Validate if folder type is not container
                if (parent.FolderType == FolderType.Container)
                {
                    Log.ForContext("Info", new { EntityId = folderToCreateDto.ParentId }, true)
                       .Information($"User : {folderToCreateDto.CreatedBy} try to create folders or containers in a container");
                    return BadRequest("can not create folders or containers in container");
                }
            }

            var folderPath = await GetFolderPath(folderToCreateDto.PartitionId, folderToCreateDto.ParentId);
            if (folderPath is null)
                return BadRequest("Couldn't Find Partition or Folder");

            var folderToAddCore = _mapper.Map<FolderCore>(folderToCreateDto);

            var isFolderDirectoryCreated = _folderService.SaveFolder(folderPath + folderToAddCore.Name);
            if (!isFolderDirectoryCreated)
                return BadRequest("Couldn't Create Folder Directory");

            var isAdded = await _folderService.CreateFolder(folderToAddCore);
            if (isAdded is null)
                return BadRequest("Couldn't Create Folder");

            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolder(int id)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User Try To Fetch Folder : {id}");

            var fetchedFolder = await _folderService.GetFolderById(id);
            if (fetchedFolder is null)
                return NotFound();

            var folderToDisplayDto = _mapper.Map<FolderToDisplayDto>(fetchedFolder);
            return Ok(folderToDisplayDto);
        }

        [HttpPut("Move")]
        public async Task<IActionResult> MoveFolder(FolderToMoveDto folderToMoveDto)
        {
            Log.ForContext("Info", new { EntityId = folderToMoveDto.FolderId }, true)
               .Information($"User: {folderToMoveDto.UpdatedBy} Try To Move Folder : {folderToMoveDto.FolderId} ");

            var fetchedFolder = await _folderService.GetFolderById(folderToMoveDto.FolderId);
            if (fetchedFolder is null)
                return NotFound();

            // Get Sourse Path
            var fetchedPartition = await _partitionSevice.GetPartitionById(fetchedFolder.PartitionId);
            if (fetchedPartition is null)
                return NotFound("Wrong Partition");

            var sourcePath = await _folderService.GetFolderPath(folderToMoveDto.FolderId, fetchedPartition.Path);

            string destinationPath;
            if (folderToMoveDto.DestinationId != null)
            {
                //Get Destination Folder Data 
                var destinationFolder = await _folderService.GetFolderById(folderToMoveDto.DestinationId ?? 0);
                if (destinationFolder is null)
                    return BadRequest("Couldn't Find The Destination Folder");
                if (destinationFolder.FolderType == FolderType.Container)
                {
                    Log.ForContext("Info", new { EntityId = folderToMoveDto.FolderId, DestinationId = folderToMoveDto.DestinationId }, true)
                       .Information($"User : {folderToMoveDto.UpdatedBy} try to move folder : {folderToMoveDto.FolderId} to container : {folderToMoveDto.DestinationId}");
                    return BadRequest("can not move folders or containers in container");
                }
                destinationPath = await _folderService.GetFolderPath(folderToMoveDto.DestinationId ?? 0, fetchedPartition.Path);
            }
            else
            {
                destinationPath = fetchedPartition.Path;
            }

            var fullDestinationPath = Path.Combine(destinationPath, fetchedFolder.Name);
            var isMoved = _folderService.MoveFolder(sourcePath, fullDestinationPath);
            if (!isMoved)
                return BadRequest("Couldn't Move Folder");

            fetchedFolder.ParentId = folderToMoveDto.DestinationId;
            fetchedFolder.UpdatedBy = folderToMoveDto.UpdatedBy;
            var isUpdated = await _folderService.UpdateFolder(folderToMoveDto.FolderId, fetchedFolder);
            if (!isUpdated)
                return BadRequest("Couldn't Update Folder");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFolder([FromRoute] int id, [FromBody] FolderToUpdateDto folderToUpdateDto)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User : {folderToUpdateDto.UpdatedBy} Try To Update Folder : {id}");

            var fetchedFolder = await _folderService.GetFolderById(id);
            if (fetchedFolder is null)
                return NotFound();

            if (folderToUpdateDto.FolderCategories is not null)
            {
                var isValid = folderToUpdateDto.FolderCategories.All(x => x.Id == null || fetchedFolder.FolderCategories.Any(f => f.Id == x.Id));
                if (!isValid)
                {
                    Log.ForContext("Info", new { EntityId = id }, true)
                       .Information($"FolderCategory Id Is Not Valid When Update Folder :{id}");
                    return BadRequest("FolderCategory Id Is Not Valid");
                }
            }
            if (folderToUpdateDto.FolderIndices is not null)
            {
                var isValid = folderToUpdateDto.FolderIndices.All(x => x.Id == null || fetchedFolder.FolderIndices.Any(f => f.Id == x.Id));
                if (!isValid)
                {
                    Log.ForContext("Info", new { EntityId = id }, true)
                       .Information($"FolderIndex Id Is Not Valid When Update Folder :{id}");
                    return BadRequest("FolderIndex Id Is Not Valid");
                }
            }

            //Rename Folder In Directory
            if (folderToUpdateDto.Name != fetchedFolder.Name)
            {
                var folderPath = await GetFolderPath(fetchedFolder.PartitionId, fetchedFolder.ParentId);
                if (folderPath is null)
                    return BadRequest("Wrong  Partition");

                var isFolderNameChanged = _folderService.MoveFolder(folderPath + fetchedFolder.Name, folderPath + folderToUpdateDto.Name);
                if (!isFolderNameChanged)
                    return BadRequest("Can't Change Folder Name");
            }
            var folderCore = _mapper.Map(folderToUpdateDto, fetchedFolder);
            var isUpdated = await _folderService.UpdateFolder(id, folderCore);

            if (!isUpdated)
                return BadRequest("Couldn't Update Folder Categories or Indices");

            return Ok();
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateFolderStatus([FromRoute] int id, [FromBody] bool status)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User Try To Change Status Of Folder : {id} ");

            var fetchedFolder = await _folderService.GetFolderById(id);
            if (fetchedFolder is null)
                return NotFound();

            fetchedFolder.IsActive = status;
            fetchedFolder.UpdatedAt = DateTime.Now;
            var isUpdated = await _folderService.UpdateFolder(id, fetchedFolder);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }
        private async Task<string> GetFolderPath(int partitionId, int? folderId)
        {
            var fetchedPartition = await _partitionSevice.GetPartitionById(partitionId);
            if (fetchedPartition is null)
                return null;

            if (folderId != null)
            {
                var parentFolder = await _folderService.GetFolderById(folderId ?? 0);
                if (parentFolder is null)
                    return null;
            }
            var fetchedPath = await _folderService.GetFolderPath(folderId ?? 0, fetchedPartition.Path);
            return fetchedPath;
        }
    }
}
