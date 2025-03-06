using Microsoft.EntityFrameworkCore;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Event> _dbSet;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Event>();
        }

        public async Task<Event> GetByIdAsync(string id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<Event>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<bool> CreateEntityAsync(Event model)
        {
            model.totalPlaces = model.Places;

            await _dbSet.AddAsync(model);

            return await SaveAsync();
        }

        public async Task<bool> UpdateEntityAsync(Event model)
        {
            _dbSet.Update(model);

            return await SaveAsync();
        }

        public async Task<bool> DeleteEntity(Event model)
        {
            var applications = _context.Application.Where(a => a.EventId == model.eventId).ToList();

            // Удалить все заявки пользователя
            _context.Application.RemoveRange(applications);

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
