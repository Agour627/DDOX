using AutoMapper;
using DDOX.API.Core.Models.Category;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        public CategoryController(IMapper mapper,
                                  ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryToCreateDto categoryToCreateDto)
        {
            Log.ForContext("Info", new { EntityId = categoryToCreateDto.PartitionId },true)
                .Information($"User: {categoryToCreateDto.CreatedBy} Try To Add New Category in Partition: {categoryToCreateDto.PartitionId}");

            var CategoryToAddCore = _mapper.Map<CategoryCore>(categoryToCreateDto);
            var isCreated = await _categoryService.CreateCategory(CategoryToAddCore);
            if (!isCreated)
                return BadRequest("Couldn't Create Category");

            return StatusCode(201);
        }

        [HttpGet("partition/{partitionId}")]
        public async Task<IActionResult> GetPartitionCategories(int partitionId)
        {
            Log.ForContext("Info",new {EntityId= partitionId },true)
               .Information($"User Try To Fetch Categories In partition :{partitionId}");

            var fetchedCategories = await _categoryService.GetPartitionCategories(partitionId);
            if (fetchedCategories == null) 
                return NotFound();

            var categoriesToDisplay = _mapper.Map<List<CategoryToDisplayDto>>(fetchedCategories);
            return Ok(categoriesToDisplay);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            Log.Information($"User Try To Fetch All Categories");

            var fetchedCategories = await _categoryService.GetAllCategories();
            if (fetchedCategories == null)
                return NotFound();

            var categoriesToDisplay = _mapper.Map<List<CategoryToDisplayDto>>(fetchedCategories);
            return Ok(categoriesToDisplay);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Log.ForContext("Info", new { EntityId = id },true)
               .Information($"User Try To Fetch Category: {id}");

            var fetchedCategory = await _categoryService.GetCategoryById(id);
            if (fetchedCategory is null)
                return NotFound();
            var categoryToDisplay = _mapper.Map<CategoryToDisplayDto>(fetchedCategory);
            return Ok(categoryToDisplay);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute]int id, [FromBody] CategoryToUpdateDto categoryToUpdateDto)
        {
            Log.ForContext("Info", new { EntityId = id }, true)
               .Information($"User: {categoryToUpdateDto.UpdatedBy} Try To Update Category :{id}");

            var fetchedCategory = await _categoryService.GetCategoryById(id);
            if (fetchedCategory is null)
                return NotFound();

            if (categoryToUpdateDto.CategoryIndices is not null)
            {
                var isValid = categoryToUpdateDto.CategoryIndices.All(x => x.Id == null || fetchedCategory.CategoryIndices.Any(f => f.Id == x.Id));
                if (!isValid)
                {
                    Log.ForContext("Info", new { EntityId = id }, true)
                       .Information($"CategoryIndex Id Is Not Valid When Update Category :{id}");

                    return BadRequest("CategoryIndex Id Is Not Valid");
                }
            }


            var categoryCore = _mapper.Map(categoryToUpdateDto, fetchedCategory);
            var isUpdated = await _categoryService.UpdateCategory(id, categoryCore);
          
            if (!isUpdated)
                return BadRequest("Couldn't Update Category ");

            return Ok();
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateCategoryStatus([FromRoute] int id, [FromBody] bool status)
        {
            Log.ForContext("Info", new { EntityId = id },true)
                .Information($"User Try To update status of Category :{id}");

            var fetchedCategory = await _categoryService.GetCategoryById(id);
            if (fetchedCategory is null)
                return NotFound();

            fetchedCategory.IsActive = status;
            fetchedCategory.UpdatedAt = DateTime.Now;
            var isUpdated = await _categoryService.UpdateCategory(id, fetchedCategory);
            if (!isUpdated)
                return BadRequest();

            return Ok();
        }

    }
}
