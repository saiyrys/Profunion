using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.IFiles;
using profunion.Applications.Interface.INews;
using profunion.Shared.Dto.News;

namespace profunion.API.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IFileService _fileService;
        public NewsController(INewsService newsService, IFileService fileService)
        {
            _newsService = newsService;
            _fileService = fileService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<GetNewsDto>>> GetNewses(int page, [FromQuery] NewsQueryDto query, SortStateNews sort)
        {
            var (newses, totalPages) = await _newsService.GetNewses(page, query, sort);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Items = newses, countPage = totalPages });
        }

        [HttpGet("{newsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNewsById(string newsId)
        {
            var news = await _newsService.GetNews(newsId);

            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateNews(CreateNewsDto createNews, CancellationToken cancellation)
        {
            if (createNews == null)
            {
                return BadRequest();
            }

            var result = await _newsService.CreateNews(createNews, cancellation);

            if (!result)
            {
                return StatusCode(418);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Новость успешно создана");

        }

        [HttpPost("views/{newsId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> IncrementNews(string newsId)
        {

            var result = await _newsService.IncrementationViews(newsId);

            if (!result)
            {
                return StatusCode(418);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(true);

        }

        [HttpPatch("{newsId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateNews(string newsId, [FromBody] UpdateNewsDto updateNews)
        {
            if (updateNews == null)
            {
                return BadRequest("Invalid data.");
            }

            var newsToUpdate = await _newsService.UpdateNews(newsId, updateNews);

            if (newsToUpdate)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("{newsId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteNews(string newsId, CancellationToken cancellation)
        {
            var result = await _newsService.DeleteNews(newsId, cancellation);

            if (!result)
            {
                return NotFound("Новость не найдено");
            }

            return Ok("Новость успешно удалена");
        }

        [HttpPost("image")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateImageNews(IFormFile image, CancellationToken cancellation)
        {
            if (image == null)
            {
                return BadRequest();
            }

            (string Id,string filename, string Url) = await _fileService.WriteFile(image, "News", cancellation);

            var result = new
            {
                id = Id,
                name = filename,
                url = Url
            };

            if (result == null)
            {
                return StatusCode(500, "Ошибка при создании медиа");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Task.Delay(1000);

            return Ok(result);
        }
               
        [HttpDelete("image/{fileName}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteImage(string fileName)
        {
            var filePath = await _fileService.DeleteFile(fileName);

            if (filePath == null)
            {
                return BadRequest("File not found");
            }

            return Ok(true);
        }
    }
}
