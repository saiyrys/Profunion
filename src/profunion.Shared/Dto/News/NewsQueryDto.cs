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

        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }

        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }
    }

    public enum SortStateNews
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        DateAsc,
        DateDesc,
    }
}
