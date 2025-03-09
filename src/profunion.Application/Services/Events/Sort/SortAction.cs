using profunion.Applications.Interface.IEvents;
using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Events.Sort
{
    public class SortAction : ISortAction
    {
        public IEnumerable<GetEventDto> SortObject(IEnumerable<GetEventDto> events, SortState? sort)
        {
            switch (sort)
            {
                case SortState.AlphabeticAsc:
                    events = events.OrderBy(e => e.title);
                    break;
                case SortState.AlphabeticDesc:
                    events = events.OrderByDescending(e => e.title);
                    break;
                case SortState.PlacesAsc:
                    events = events.OrderBy(e => e.places);
                    break;
                case SortState.PlacesDesc:
                    events = events.OrderByDescending(e => e.places);
                    break;
                case SortState.DateAsc:
                    events = events.OrderBy(e => e.date);
                    break;
                case SortState.DateDesc:
                    events = events.OrderByDescending(e => e.date);
                    break;
                default:
                    events = events.OrderBy(e => e.title);
                    break;
            }

            return events.ToList();
        }
    }
}
