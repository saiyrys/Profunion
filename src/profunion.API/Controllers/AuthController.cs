using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.IAuth;
using profunion.Applications.Interface.IEmailService;
using profunion.Domain.Models.UserModels;
using profunion.Shared.Dto.Auth;
using profunion.Shared.Dto.Auth.PWD;


namespace profunion.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailAuthSender _emailSender;
        private readonly IControl<User> _control;
        private readonly IResetPasswordMethods _passwordService;

        public AuthController(IAuthService authService, IResetPasswordMethods passwordService, IEmailAuthSender emailSender, IControl<User> control)
        {
            _authService = authService;
            _passwordService = passwordService;
            _emailSender = emailSender;
            _control = control;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(loginUser.password))
            {
                ModelState.AddModelError(" ", "Пароль не может быть пустым");
                return StatusCode(422, ModelState);
            }

            var login = await _authService.Login(loginUser);

            return Ok(login);
        }

        [HttpPost("email/get_code")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCodeForEmail([FromBody]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(" ", "Почта не может быть пустой");
                return StatusCode(422, ModelState);
            }

            await _emailSender.SendAuthorizationCode(email);

            return Ok("Код отправлен");
        }

        [HttpPost("email/login")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LoginUserForEmail([FromBody]string email, string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(" ", "Почта не может быть пустой");
                return StatusCode(422, ModelState);
            }

            var login = await _authService.LoginEmail(email, code);

            return Ok(login);
        }

        [HttpPost("register")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RegistrationUser(RegistrationDto registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(registration.userName))
            {
                ModelState.AddModelError(" ", "Пoле имени не может быть пустым");
                return StatusCode(422, ModelState);
            }

            if (string.IsNullOrEmpty(registration.password))
            {
                ModelState.AddModelError(" ", "Пoле пароля не может быть пустым");
                return StatusCode(422, ModelState);
            }

            await _authService.Registration(registration);

            return Ok(true);
        }

        [HttpGet("access-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserInfoByToken()
        {
            var user = await _authService.GetUser();

            // Если пользователя не нашли — ошибка 404
            if (user == null)
            {
                return NotFound(new { message = "User not found or token is invalid." });
            }

            return Ok(new ReturnRole { role = user.role });

        }

        [HttpPost("login/access-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNewToken()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = HttpContext.Request.Cookies["refreshToken"];


            if (string.IsNullOrEmpty(token))
                return BadRequest("Unathorized");

            var response = await _authService.GetNewTokens(token);

            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _passwordService.ChangePassword(dto);

            return Ok(true);
        }

        [HttpPost("logout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> logout()
        {
            Response.Cookies.Delete("refreshToken");

            return Ok(true);
        }

        [HttpPost("email/request-reset")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EmailForReset([FromBody] SendEmailReset dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(dto.email))
            {
                ModelState.AddModelError(" ", "Почта не может быть пустой");
                return StatusCode(422, ModelState);
            }

            await _passwordService.RequestPasswordReset(dto.email);

            return Ok("письмо успешно отправлено");
        }

        [HttpPost("email/reset-password/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ResetPassword(string token, [FromBody] ResetNewPassword dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(" ", "Почта не может быть пустой");
                return StatusCode(422, ModelState);
            }

            await _passwordService.ResetPasswordByToken(token, dto.newPassword);

            return Ok("Пароль успешно изменен");
        }
    }
}
