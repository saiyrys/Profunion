using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IAuth
{
    public interface IControl<TUser>
    {
        Task<TUser> FindByIdAsync(string userId);
        Task<TUser> FindByNameAsync(string userName);
        Task<TUser> FindByEmailAsync(string mail);
        Task<TUser> FindByTokenAsync(string token);
        Task<string> VerifyByTokenAsync();
    }
}
