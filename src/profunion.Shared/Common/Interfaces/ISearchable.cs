using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Common.Interfaces
{
    public interface ISearchable
    {
        IEnumerable<string> GetSearchableFields();
    }
}
