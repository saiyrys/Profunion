using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Events
{
    public class EventQueryDto
    {
        public string? search { get; set; }

        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }

        public bool? isActive { get; set; }

        public bool? status { get; set; }
    }

    public enum SortState
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        DateAsc,
        DateDesc,
        PlacesAsc,
        PlacesDesc
    }
}
