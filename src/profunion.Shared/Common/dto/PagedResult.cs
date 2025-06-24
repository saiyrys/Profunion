using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Common.dto
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
    }
}
