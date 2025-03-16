using profunion.Shared.Dto.Auth;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IAuth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginUserDto loginUser);

        Task<LoginResponseDto> LoginEmail(string email, string code);
        Task<bool> Registration(RegistrationDto registration);
        Task<UserInfoDto> GetUser();
        Task<LoginResponseDto> GetNewTokens(string refreshToken);
    }
}
