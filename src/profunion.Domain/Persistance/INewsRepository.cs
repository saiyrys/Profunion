using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.NewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface INewsRepository
    {
        Task<News> GetByIdAsync(string id);

        Task<IEnumerable<News>> GetAllAsync();

        Task<bool> CreateAsync(News model, CancellationToken cancellation);

        Task<bool> UpdateAsync(News model);

        Task<bool> SaveAsync();

        Task<bool> DeleteAsync(News model);
    }
}
