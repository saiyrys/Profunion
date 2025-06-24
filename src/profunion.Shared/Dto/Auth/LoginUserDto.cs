using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Auth
{
    public class LoginUserDto
    {
        public string userName { get; set; }

        public string password { get; set; }
    }
}
