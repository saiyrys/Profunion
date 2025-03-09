using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.IFiles;

namespace profunion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publicController : Controller
    {
        private readonly IFileService _fileService;

        public publicController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("uploads/news/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImageNews(string fileName)
        {
            var filePath = await _fileService.OpenFile(fileName);

            if (filePath != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                return File(fileStream, "image/png");
            }
            else
            {
                return BadRequest("File not found");
            }
        }

        [HttpGet("uploads/event/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImageEvent(string fileName)
        {
            var filePath = await _fileService.OpenFile(fileName);

            if (filePath != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                return File(fileStream, "image/png");
            }
            else
            {
                return BadRequest("File not found");
            }
        }

    }
}
