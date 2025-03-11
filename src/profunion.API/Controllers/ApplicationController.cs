using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using profunion.Applications.Interface.IApplications;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Application;
using profunion.Shared.Dto.Users;

namespace profunion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class applicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        public applicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetApplication(int page, [FromQuery]ApplicationQueryDto query, SortStateApplication sort)
        {
            var (application, totalPages) = await _applicationService.GetApplication(page, query, sort);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { Items = application, countPage = totalPages });
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUserApplication(string userId, int page)
        {
            long Id = long.Parse(userId);
            var (userApplication, totalPages) = await _applicationService.GetUserApplication(Id, page);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { Items = userApplication, countPages = totalPages });
        }

        [HttpGet("report")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetApplicationReport()
        {
            var applications = await _applicationService.GetApplicationForReport();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                
                var worksheet = package.Workbook.Worksheets.Add("Отчёт по заявкам");

                worksheet.Cells[1, 1].Value = "Фамилия";
                worksheet.Cells[1, 2].Value = "Имя";
                worksheet.Cells[1, 3].Value = "Отчество";
                worksheet.Cells[1, 4].Value = "Мероприятие";
                worksheet.Cells[1, 5].Value = "Заявок на данное мероприятие";

                int row = 2;
                foreach (var application in applications)
                {
                    worksheet.Cells[row, 1].Value = application.user.lastName;
                    worksheet.Cells[row, 2].Value = application.user.firstName;
                    worksheet.Cells[row, 3].Value = application.user.middleName;
                    worksheet.Cells[row, 4].Value = string.Join(", ", application.@event.title);
                    worksheet.Cells[row, 5].Value = application.places;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"application_report_{DateTime.Now:yyyy:MM:dd:HH:mm}.xlsx";
               /* Response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";
                Response.Headers["Content-Type"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";*/

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto createApplication)
        {
            await _applicationService.CreateApplication(createApplication);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createApplication == null)
                return BadRequest();

            return Ok("Заявка на мероприятие успешно создана");

        }
    }
}
