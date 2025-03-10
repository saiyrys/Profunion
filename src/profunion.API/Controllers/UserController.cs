using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using profunion.Applications.Interface.IAuth;
using profunion.Applications.Interface.IUser;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Persistance.Repository;
using profunion.Shared.Dto.Users;

namespace profunion.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService,IAuthService authService, IUserRepository userRepository)
        {
            _userService = userService;
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpGet()]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUsersByAdmin(int page, [FromQuery] UserQueryDto query, SortStateUser sort)
        {
            var (users, totalPages) = await _userService.GetUsersByAdmin(page, query, sort);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Items = users, countPage = totalPages });

        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(string userId)
        {
            long Id = long.Parse(userId);

            var profile = await _userService.GetUserById(Id);

            if (profile == null)
            {
                return NotFound("Профиль пользователя не найден");
            }

            return Ok(profile);
        }

        [HttpGet("profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserProfile()
        {
            string token = HttpContext.Request.Headers["authorization"];

            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
                var user = await _authService.GetUser(token);
                
                return Ok(user);
            }

            return NoContent();
        }

        [HttpGet("report")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetEventsStatusReport()
        {
            var users = await _userRepository.GetAllAsync(); // Получаем список мероприятий

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets.Add("Пользователи");

                worksheet.Cells[1, 1].Value = "НикНейм";
                worksheet.Cells[1, 2].Value = "Фамилия";
                worksheet.Cells[1, 3].Value = "Имя";
                worksheet.Cells[1, 4].Value = "Отчество";
                worksheet.Cells[1, 5].Value = "Почта";
                worksheet.Cells[1, 6].Value = "Роль";

                int row = 2;
                foreach (var us in users)
                {
                    worksheet.Cells[row, 1].Value = us.userName;
                    worksheet.Cells[row, 2].Value = us.lastName;
                    worksheet.Cells[row, 3].Value = us.firstName;
                    worksheet.Cells[row, 4].Value = us.middleName;
                    worksheet.Cells[row, 5].Value = us.email;
                    worksheet.Cells[row, 6].Value = us.role;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"user_report_{DateTime.Now:yyyy:MM:dd:HH:mm}.xlsx";
                Response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";
                Response.Headers["Content-Type"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpPatch("{userId}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto updateUser)
        {
            long Id = long.Parse(userId); 
            if (updateUser == null)
            {
                return BadRequest("Пользователь не найден");
            }

            var userToUpdate = await _userService.UpdateUsers(Id, updateUser);

            if (userToUpdate)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            long Id = long.Parse(userId);
            var result = await _userService.DeleteUser(Id);

            if (!result)
            {
                return NotFound("Пользователь не найден");
            }

            return Ok("Пользователь удалён");
        }
    }
}
