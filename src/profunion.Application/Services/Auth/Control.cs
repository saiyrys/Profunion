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
            if (HttpContext.HttpContext.Request.Headers == null)
            {
                /*var cookieToken = HttpContext.HttpContext.Request.Headers["accessToken"];

                await FindByTokenAsync(cookieToken);*/
            }

            string token = HttpContext.HttpContext.Request.Headers["authorization"];

            if (token.StartsWith("Bearer"))
                token = token.Substring("Bearer ".Length).Trim();

            return token;
            
            
        }
    }
}
