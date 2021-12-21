using AutoMapper;
using DDOX.API.Core.Models.Page;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Page;
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
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;
        private readonly IMapper _mapper;

        public PageController(IPageService pageService,
                             IMapper mapper)
        {
            _pageService = pageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddPage(PageToAddDto pageToAddDto)
        {
            Log.ForContext("Info", new { PageNumber = pageToAddDto.PageNumber, FileId = pageToAddDto.FileId }, true)
                .Information($"User try to save page number: {pageToAddDto.PageNumber} in file: {pageToAddDto.FileId}");

            var pageCore = _mapper.Map<PageCore>(pageToAddDto);
            var createdPage = await _pageService.SavePage(pageCore);
            if (createdPage is null)
                return BadRequest("Couldn't create Page");
            return StatusCode(201);
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetPages([FromRoute] int fileId)
        {
            Log.ForContext("Info", new { FileId = fileId }, true)
                .Information($"User try to fetch pages in file: {fileId}");

            var fetchedPages = await _pageService.GetPagesByFileId(fileId);
            if (fetchedPages == null)
                return NotFound();

            var folderToDisplayDto = _mapper.Map<List<PageDto>>(fetchedPages);
            return Ok(folderToDisplayDto);
        }

    }
}
