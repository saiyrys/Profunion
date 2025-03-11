using profunion.Domain.Models.ApplicationModels;
using profunion.Domain.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface IApplicationRepository
    {
        Task<Application> GetByIdAsync(long id);

        Task<ICollection<Application>> GetAllAsync();

        Task<bool> CreateEntityAsync(Application model, int places);

        Task<bool> UpdateEntityAsync(Application model);

        Task<bool> SaveAsync();

        Task<bool> DeleteEntity(Application model);
    }
}
