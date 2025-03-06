using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Users
{
    public class UserQueryDto
    {
        public string? search { get; set; }
    }

    public enum SortStateUser
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
    }
}
