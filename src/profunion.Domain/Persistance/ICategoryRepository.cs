using profunion.Domain.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface ICategoryRepository
    {
        Task<Categories> GetByIdAsync(string id);

        Task<IEnumerable<Categories>> GetAllAsync();

        Task<bool> CreateEntityAsync(Categories model);

        Task<bool> UpdateEntityAsync(Categories model);

        Task<bool> SaveAsync();

        Task<bool> DeleteEntity(Categories model);

        Task<bool> RemoveRange(string categoryId);
    }
}
