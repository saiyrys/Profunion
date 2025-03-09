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

        public string? created_at_start { get; set; }

        public string? created_at_end { get; set; }

        public string? updated_at_start { get; set; }

        public string? updated_at_end { get; set; }



        public string? role { get; set; }
    }

    public enum SortStateUser
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        CreatedAsc,
        CreatedDesc,
        UpdatedAsc,
        UpdatedDesc
    }
}
