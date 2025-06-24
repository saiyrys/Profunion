using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IEvents
{
    public interface ISortAction
    {
        public IEnumerable<GetEventDto> SortObject(IEnumerable<GetEventDto> events, SortState? sort);
    }
}
