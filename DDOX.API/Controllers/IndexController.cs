using AutoMapper;
using DDOX.API.Core.Models.Index;
using DDOX.API.Core.Models.IndexRestriction;
using DDOX.API.Core.Services;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Index;
using DDOX.API.Dtos.IndexRestection;
using DDOX.API.Dtos.Restriction;
using Microsoft.AspNetCore.Http;
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
    public class IndexController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIndexService _indexService;
        private readonly IRestrictionService _restrictionService;
        public IndexController(IMapper mapper,
                               IIndexService indexService,
                               IRestrictionService restrictionService)
        {
            _mapper = mapper;
            _indexService = indexService;
            _restrictionService = restrictionService;
        }

        [HttpGet("partition/{partitionId}")]
        public async Task<IActionResult> GetPartitionIndices([FromRoute] int partitionId)
        {
            Log.ForContext("Info", new { EntityId = partitionId }, true)
                .Information($"User Try To Fetch  indices In partition: {partitionId}");

            var fetchedIndices = await _indexService.GetPartitionIndices(partitionId);
            if (fetchedIndices == null)
                return NotFound();

            var indicesToDisplay = _mapper.Map<List<IndexToDisplayDto>>(fetchedIndices);
            return Ok(indicesToDisplay);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIndices()
        {
            Log.Information($"User Try To Fetch All Indices");

            var fetchedIndices = await _indexService.GetAllIndices();
            if (fetchedIndices == null)
                return NotFound();

            var indicesTodisplay = _mapper.Map<List<IndexToDisplayDto>>(fetchedIndices);
            return Ok(indicesTodisplay);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIndex([FromRoute] int id)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
                .Information($"User Try To Fetch Index: {id}");

            var fetchedIndex = await _indexService.GetIndexById(id);
            if (fetchedIndex is null)
                return NotFound();

            var indexToDisplay = _mapper.Map<IndexToDisplayDto>(fetchedIndex);
            return Ok(indexToDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndex([FromBody] IndexToCreateDto indexToCreateDto)
        {
            Log.Information($"User:{indexToCreateDto.CreatedBy} Try To Add New Index");

            var indexCore = _mapper.Map<IndexCore>(indexToCreateDto);
            var createdIndex = await _indexService.CreateIndex(indexCore);
            if (!createdIndex)
                return BadRequest("Couldn't create Index");
            return StatusCode(201);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateIndexStatus([FromRoute] int id, [FromBody] bool status)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
                .Information($"User Try To update status of Category :{id}");

            var fetchedIndex = await _indexService.GetIndexById(id);
            if (fetchedIndex is null)
                return NotFound();

            fetchedIndex.IsActive = status;
            fetchedIndex.UpdatedAt = DateTime.Now;
            var isUpdated = await _indexService.UpdateIndex(id, fetchedIndex);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIndex([FromRoute] int id, [FromBody] IndexToUpdateDto indexToUpdateDto)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
                .Information($"User: {indexToUpdateDto.UpdatedBy} Try To Update Category :{id}");

            var fetchedIndex = await _indexService.GetIndexById(id);
            if (fetchedIndex is null)
                return NotFound("The Index is not exist");

            if (indexToUpdateDto.IndexRestrictions is not null)
            {
                var isValid = indexToUpdateDto.IndexRestrictions.All(x => x.Id == null || fetchedIndex.IndexRestrictions.Any(f => f.Id == x.Id));
                if (!isValid)
                {
                    Log.ForContext("Info", new { EntityId = id }, true)
                        .Information($"IndexRestriction Id Is Not Valid When Update Index :{id}");

                    return BadRequest("IndexRestriction Id Is Not Valid");
                }
            }

            var indexToUpdateCore = _mapper.Map(indexToUpdateDto, fetchedIndex);
            var isUpdated = await _indexService.UpdateIndex(id, indexToUpdateCore);

            if (!isUpdated)
                return BadRequest("Couldn't Update Index");
            return Ok();
        }

        [HttpGet("Restrictions")]
        public async Task<IActionResult> GetAllRestrictions()
        {
            Log.Information($"User Try To Fetch All Restrictions");

            var fetchedRestrictions = await _restrictionService.GetAllRestrictions();
            if (fetchedRestrictions == null)
                return NotFound();

            var restrictionsToDisplay = _mapper.Map<List<RestrictionToDisplayDto>>(fetchedRestrictions);
            return Ok(restrictionsToDisplay.OrderBy(e => e.Id));
        }

    }
}
