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

        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
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
