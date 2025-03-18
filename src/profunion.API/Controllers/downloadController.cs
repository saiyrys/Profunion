using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace profunion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class downloadController : Controller
    {
        [HttpGet()]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        public IActionResult DownloadInstaller()
        {
            var fileName = "profunion-setup-1.2.8.exe";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "installers", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Файл не найден");
            }

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }
    }
}
