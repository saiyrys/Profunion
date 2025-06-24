using AutoMapper;
using Microsoft.EntityFrameworkCore;
using profunion.Applications.Interface.ICategory;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Infrastructure.Persistance.Repository;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Category;


namespace profunion.Applications.Services.Category
{
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _categoryRepository;
        IPagination _pagination;
        IUpdateMethods _update;
        IMapper _mapper;

        public CategoryService(ICategoryRepository category, IPagination pagination, IUpdateMethods update, IMapper mapper)
        {
            _categoryRepository = category;
            _pagination = pagination;
            _update = update;
            _mapper = mapper;
        }

        public async Task<bool> CreateCategories(CreateCategoriesDto category)
        {
            if (category is null)
                throw new ArgumentException("Unfilled");

            var categoryDto = _mapper.Map<Categories>(category);

            categoryDto.Id = Guid.NewGuid().ToString();

            await _categoryRepository.CreateEntityAsync(categoryDto);

            return true;

        }
        public async Task<(IEnumerable<CategoriesDto>, int TotalPages)> GetAllCategories(int page, string? search)
        {
            int items = 18;

            var categories = await _categoryRepository.GetAllAsync();
            var categoriesDto = _mapper.Map<IEnumerable<CategoriesDto>>(categories);

            if(!string.IsNullOrEmpty(search))
            {
                categoriesDto = await Search<CategoriesDto>.SearchEntities(categoriesDto, search);
            }

            if (categories is null)
                throw new ArgumentNullException("Categories not found");

            var pagianteItem = await _pagination.Paginate(categoriesDto, page);
            categoriesDto = pagianteItem.Items;

            int totalPages = pagianteItem.TotalPages;
            
            return (categoriesDto, totalPages);
        }

        public Task<CategoriesDto> GetCategory(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCategory(string id, UpdateCategoriesDto updateDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            await _update.UpdateEntity<Categories, UpdateCategoriesDto, string>(id, updateDto);

            if (!await _categoryRepository.UpdateEntityAsync(category))
            {
                throw new ArgumentException("Ошибка при обновлении категории");
            }

            return true;

        }


        public async Task<bool> DeleteCategories(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("uncorrect id");

            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);


            // Удаляем саму категорию
            await _categoryRepository.DeleteEntity(categoryToDelete);

            return true;
        }

    }
}
