using profunion.Domain.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(string id);

        Task<IEnumerable<Event>> GetAllAsync();

        Task<bool> CreateEntityAsync(Event model);

        Task<bool> UpdateEntityAsync(Event model);

        Task<bool> SaveAsync();

        Task<bool> DeleteEntity(Event model);

        /*Task<bool> DeleteRangeEntities(List<T> models);*/
    }
}
