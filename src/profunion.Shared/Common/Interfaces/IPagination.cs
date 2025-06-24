using profunion.Shared.Common;
using profunion.Shared.Common.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Common.Interfaces
{
    public interface IPagination
    {
        Task<PagedResult<T>> Paginate<T>(IEnumerable<T> items, int page = 1);
    }
}
