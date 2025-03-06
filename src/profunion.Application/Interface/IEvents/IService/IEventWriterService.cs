using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IEvents.IService
{
    public interface IEventWriterService
    {
        Task<bool> CreateEvents(CreateEventDto eventsCreate, CancellationToken cancellation);
        Task<bool> UpdateEvents(string eventId, UpdateEventDto updateEvent, CancellationToken cancellation);
        Task<bool> DeleteEvents(string eventId, CancellationToken cancellation);
    }
}
