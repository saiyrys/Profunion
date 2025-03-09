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

        public string? date_start { get; set; }
        public string? date_end { get; set; }

        public string? time_start { get; set; }
        public string? time_end { get; set; }

        public bool? isActive { get; set; }

        public bool? status { get; set; }
    }

    public enum SortState
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        PlacesAsc,
        PlacesDesc, 
        DateAsc,
        DateDesc
    }
}
