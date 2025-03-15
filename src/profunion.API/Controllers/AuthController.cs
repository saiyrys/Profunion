using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.IAuth;
using profunion.Applications.Interface.IEmailService;
using profunion.Shared.Dto.Auth;


namespace profunion.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordService _passwordService;

        public AuthController(IAuthService authService, IPasswordService passwordService, IEmailSender emailSender)
        {
            _authService = authService;
            _passwordService = passwordService;
            _emailSender = emailSender;
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
            var token = HttpContext.Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(token) && HttpContext.Request.Headers.ContainsKey("authorization"))
            {
                var authHeader = HttpContext.Request.Headers["authorization"].ToString();
                Console.WriteLine($"Authorization header: {authHeader}"); // Лог для проверки

                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }

                var userT = await _authService.GetUser(token);

                var roleT = new ReturnRole
                {
                    role = userT.role
                };

                return Ok(roleT);
            }

            var user = await _authService.GetUser(token);

            var role = new ReturnRole
            {
                role = user.role
            };

            return Ok(role);


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

        [HttpPost("change_password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangePassword(string Id, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = HttpContext.Request.Cookies["accessToken"];


            if (string.IsNullOrEmpty(token))
                return BadRequest("Unathorized");

            long userId = long.Parse(Id);

            await _passwordService.ChangePassword(token, userId, password);

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
    }
}
