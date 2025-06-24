using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public UserInfoDto User { get; set; }
    }
}
