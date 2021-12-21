using AutoMapper;
using DDOX.API.Core.Models.Category;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IGenericRepository<Category> categoryRepository,
                               IMapper mapper,
                               IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper  = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateCategory(CategoryCore categoryCore)
        {
            var category = _mapper.Map<Category>(categoryCore);
            var addedCategory = await _categoryRepository.Add(category);
            var isAdded= await _unitOfWork.Save();
            if (isAdded)
                Log.ForContext("Info", new { EntityId = addedCategory.Id}, true)
                   .Information($"User: {categoryCore.CreatedBy} Successfully Added Category: {addedCategory.Id}");
            return isAdded;
        }

        public async Task<List<CategoryCore>> GetPartitionCategories(int partitionId)
        {
            var fetchedCategories = await _categoryRepository.FindByCondition(d => d.PartitionId == partitionId && d.IsActive);

            if (fetchedCategories.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = partitionId }, true)
                   .Information($"Partition: {partitionId} Doesn't Contain Categories");
                return null;
            }
            return _mapper.Map<List<CategoryCore>>(fetchedCategories);
        }

        public async Task<CategoryCore> GetCategoryById(int id)
        {
            var fetchedCategory = await _categoryRepository.FindByCondition(c => c.Id == id,
                                                                            c => c.Include(category => category.CategoryIndices));
            if(fetchedCategory.Count == 0 )
            {
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"Database Doesn't Contain Category: {id}");
                return null;
            }
            return _mapper.Map<CategoryCore>(fetchedCategory.FirstOrDefault());
        }

        public async Task<bool> UpdateCategory(int id, CategoryCore categoryCore)
        {
            var oldCategory = await _categoryRepository.FindByCondition(category => category.Id == id,
                                                                       relation => relation.Include(category => category.CategoryIndices));

            var categoryToUpdate = _mapper.Map(categoryCore, oldCategory.FirstOrDefault());
            var updatedCategory = await _categoryRepository.Update(categoryToUpdate);

            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new{ EntityId = id}, true)
                   .Information($"User: {categoryCore.UpdatedBy} Successfully Updated Category: {id}");
            return isUpdated;
        }

        public async Task<List<CategoryCore>> GetAllCategories()
        {
            var fetchedCategories = await _categoryRepository.GetAll();
            if (fetchedCategories.LongCount() == 0)
            {
                Log.Information($"Database Doesn't Contain Categories");
                return null;
            }
            return _mapper.Map<List<CategoryCore>>(fetchedCategories);
        }
    }
}
