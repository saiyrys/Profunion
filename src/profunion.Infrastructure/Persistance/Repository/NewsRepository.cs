using Microsoft.EntityFrameworkCore;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.NewsModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<News> _dbSet;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<News>();
        }

        public async Task<News> GetByIdAsync(string id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<News>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<bool> CreateAsync(News model, CancellationToken cancellation)
        {
            await _dbSet.AddAsync(model, cancellation);

            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(News model)
        {
            _dbSet.Update(model);

            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(News model)
        {
            _dbSet.Remove(model);

            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
