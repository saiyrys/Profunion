using profunion.Domain.Models.EventModels;
using profunion.Shared.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.ICategory
{
    public interface ICategoryService
    {
        Task<bool> CreateCategories(CreateCategoriesDto category);

        Task<(IEnumerable<CategoriesDto>, int TotalPages)> GetAllCategories(int page);

        Task<CategoriesDto> GetCategory(string id);

        Task<bool> UpdateCategory(string id, UpdateCategoriesDto updateDto);

        Task<bool> DeleteCategories(string id);
    }
}
