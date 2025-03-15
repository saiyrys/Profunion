using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IAuth
{
    public interface IPasswordService
    {
        Task<bool> ChangePassword(string token, long userId, string password);
    }
}
