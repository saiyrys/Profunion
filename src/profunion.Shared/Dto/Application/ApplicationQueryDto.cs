using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Application
{
    public class ApplicationQueryDto
    {
        public string? search { get; set; }
    }

    public enum SortStateApplication
    {
        Current,
        PlacesAsc,
        PlacesDesc
    }
}
