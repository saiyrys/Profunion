using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Auth.PWD
{
    public class ChangePasswordDto
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
