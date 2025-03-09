using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.News
{
    public class NewsQueryDto
    {
        public string? search { get; set; }

        public string? created_at_start { get; set; }
        public string? created_at_end { get; set; }

    }

    public enum SortStateNews
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        ViewsAsc,
        ViewsDesc,
        DateAsc,
        DateDesc,
    }
}
