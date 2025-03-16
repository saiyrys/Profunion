using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using profunion.Applications.Interface.IAuth;
using profunion.Domain.Constants;
using profunion.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;

namespace profunion.Applications.Services.Auth
{
    public class Control<TUser> : IControl<TUser> where TUser : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor HttpContext;

        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        public Control(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;

            HttpContext = httpContext;

        }
        public virtual async Task<TUser> FindByIdAsync(string userId)
        {
            return await _context.Set<TUser>().FirstOrDefaultAsync(u => EF.Property<string>(u, "userId") == userId, CancellationToken);
        }

        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            return await _context.Set<TUser>().FirstOrDefaultAsync(u => EF.Property<string>(u, "userName") == userName, CancellationToken);
        }

        public virtual async Task<TUser> FindByEmailAsync(string mail)
        {
            return await _context.Set<TUser>().FirstOrDefaultAsync(u => EF.Property<string>(u, "email") == mail, CancellationToken);
        }
        public virtual async Task<TUser> FindByTokenAsync(string token)
        {
            string key = Auth_Constants.JWT_SECRET_KEY;

            var tokenHandler = new JwtSecurityTokenHandler();

            var readToken = tokenHandler.ReadJwtToken(token);

            var claims = readToken.Claims;

            string userId = claims.FirstOrDefault(x => x.Type == "nameid" || x.Type == "sub")?.Value;

            TUser user = await FindByIdAsync(userId);

            return user;
        }

        public async Task<string> VerifyByTokenAsync()
        {
            var request = HttpContext.HttpContext.Request;

            // Проверяем сначала куки
            var token = request.Cookies["accessToken"];

            // Если в куках нет, пробуем заголовок
            if (string.IsNullOrEmpty(token) && request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                }
            }

            return token;
        }
    }
}
