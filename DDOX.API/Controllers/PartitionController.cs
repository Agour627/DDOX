using AutoMapper;
using DDOX.API.Core.Models.Partition;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Folder;
using DDOX.API.Dtos.Partition;
using DDOX.API.Dtos.Partiton;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPartitionSevice _partitionSevice;
        private readonly IFolderService _folderService;
        public PartitionController(IMapper mapper,
                                   IPartitionSevice partitionSevice,
                                   IFolderService folderService)
        {
            _mapper = mapper;
            _partitionSevice = partitionSevice;
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartition([FromBody] PartitionToCreateDto partitionToCreate)
        {
            Log.Information($"User : {partitionToCreate.CreatedBy} Try To Add New Partition");
            var partitionIsDuplicated = await _partitionSevice.CheckPartitionPathDuplication(partitionToCreate.Path);
            if (partitionIsDuplicated)
            {
                Log.ForContext("Info", new { Path = partitionToCreate.Path }, true)
                   .Information($"another partition with the same path : ({partitionToCreate.Path}) is already exists");
                return BadRequest($"another partition with the same path : ({partitionToCreate.Path}) is already exists");
            }
            var partitionCore = _mapper.Map<PartitionCore>(partitionToCreate);
            var createdPartition = await _partitionSevice.CreatePartition(partitionCore);
            if (!createdPartition)
                return BadRequest("a problem occurred when Creating this partition");
            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartitions()
        {
            Log.Information($"User Try To fetch Partitions ");

            var fetchedPartitions = await _partitionSevice.GetAllPartitions();
            if (fetchedPartitions == null)
                return NotFound();

            var partitionsToDisplayDto = _mapper.Map<List<PartitionToDisplayDto>>(fetchedPartitions);
            return Ok(partitionsToDisplayDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartition([FromRoute] int id)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User Try To Fetch Partition : {id}");

            var fetchedPartition = await _partitionSevice.GetPartitionById(id);
            if (fetchedPartition is null)
                return NotFound();

            var partitionToDisplay = _mapper.Map<PartitionToDisplayDto>(fetchedPartition);
            var fetchedFolders = await _folderService.GetFoldersInPartition(id);
            partitionToDisplay.Folders = _mapper.Map<List<FolderToDisplayDto>>(fetchedFolders);
            return Ok(partitionToDisplay);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePartition([FromRoute] int id, [FromBody] PartitionToUpdateDto partitionToUpdateDto)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
                .Information($"User : {partitionToUpdateDto.UpdatedBy} Try To Update Partition : {id}");

            var fetchedPartition = await _partitionSevice.GetPartitionById(id);
            if (fetchedPartition is null)
                return NotFound();

            var partitionToUpdate = _mapper.Map(partitionToUpdateDto, fetchedPartition);
            var isUpdated = await _partitionSevice.UpdatePartition(id, partitionToUpdate);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }

    }
}
