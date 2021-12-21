using AutoMapper;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Extension;
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
    public class ExtensionController : ControllerBase
    {
        private readonly IExtensionService _extensionService;
        private readonly IMapper _mapper;
        public ExtensionController(IExtensionService extensionService, IMapper mapper)
        {
            _extensionService = extensionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExtentions()
        {
            Log.Information($"User tried to fetch Extensions");
            var fetchedExtensions = await _extensionService.GetAllExtentions();
            if (fetchedExtensions == null)
                return NotFound();
            var extesionsToDisplay = _mapper.Map<List<ExtensionToDisplayDto>>(fetchedExtensions);
            return Ok(extesionsToDisplay);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExtension([FromRoute] int id, [FromBody] bool status)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
              .Information($"User tried to update Extension: {id}");

            var fetchedExtension = await _extensionService.GetExtensionById(id);
            if (fetchedExtension is null)
                return NotFound();

            fetchedExtension.IsAllowed = status;
            var isUpdated = await _extensionService.UpdateExtension(id, fetchedExtension);
            if (!isUpdated)
                return BadRequest("Couldn't Update Extension");
            return Ok(isUpdated);
        }



    }
}
