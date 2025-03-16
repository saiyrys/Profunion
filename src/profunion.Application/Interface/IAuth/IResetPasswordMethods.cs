using profunion.Shared.Dto.Auth.PWD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IAuth
{
    public interface IResetPasswordMethods
    {
        Task RequestPasswordReset(string email);

        Task<bool> ChangePassword(ChangePasswordDto dto);

        Task<bool> ResetPasswordByToken(string token, string newPassword);
    }
}
