using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IEvents.IService
{
    public interface IEventReaderService
    {
        Task<(IEnumerable<GetEventDto> Events, int TotalPages)> GetEvents(int page, EventQueryDto query, SortState sort);
        /*        Task<ICollection<GetEventDto>> GetEventsWithCategory();*/
        Task<GetEventDto> GetEventsByID(string eventId);
        /*        Task<bool> confirmLink(string eventId);*/

        Task<IEnumerable<GetEventDto>> GetFullEventData();

    }
}
