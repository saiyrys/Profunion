using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Shared.Dto.Uploads;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<User> GetByIdAsync(long id) => await _context.Users
            .FirstOrDefaultAsync(u => u.userId == id);

        public async Task<IEnumerable<User>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<bool> CreateAsync(User model)
        {
            await _dbSet.AddAsync(model);

            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(User model)
        {
            _dbSet.Update(model);

            return await SaveAsync();
        }

        public async Task<bool> Delete(User model)
        {
            // Найти все заявки, связанные с пользователем
            var applications = _context.Application.Where(a => a.UserId == model.userId).ToList();

            // Удалить все заявки пользователя
            _context.Application.RemoveRange(applications);

            _dbSet.Remove(model);

            // Сохранить изменения в базе данных
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
