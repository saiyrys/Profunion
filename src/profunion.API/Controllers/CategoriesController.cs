using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.ICategory;
using profunion.Shared.Dto.Category;
using profunion.Shared.Dto.Events;
using SendGrid.Helpers.Errors.Model;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace profunion.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : Controller
    {
        ICategoryService _category;

        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create(CreateCategoriesDto category)
        {
            if (category is null)
                throw new BadRequestException("unfilled");

            var categories = await _category.CreateCategories(category);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll(int page)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var (categories, totalPages) = await _category.GetAllCategories(page);

            return Ok(categories);
        }

        [HttpPatch("{categoryId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateEvents(string categoryId, [FromBody]UpdateCategoriesDto updateCategory)
        {
            if (updateCategory == null)
            {
                return BadRequest("Invalid data.");
            }

            var categoryToUpdate = await _category.UpdateCategory(categoryId, updateCategory);

            if (categoryToUpdate)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("categoryId")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            var deletedCategory = await _category.DeleteCategories(categoryId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(true);
        }
    }
}
