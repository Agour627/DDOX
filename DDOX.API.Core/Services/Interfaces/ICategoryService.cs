using DDOX.API.Core.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
   public interface ICategoryService
    {
        Task<bool> CreateCategory(CategoryCore categoryCore);
        Task<bool> UpdateCategory(int id, CategoryCore categoryCore);
        Task<CategoryCore> GetCategoryById(int id);
        Task<List<CategoryCore>> GetPartitionCategories(int partitionId);
        Task<List<CategoryCore>> GetAllCategories();
    }
}
