using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(long id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<bool> CreateAsync(User model);

        Task<bool> UpdateAsync(User model);

        Task<bool> SaveAsync();

        Task<bool> Delete(User model);
    }
}
