using Microsoft.EntityFrameworkCore;
using profunion.Domain.Models.ApplicationModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Application> _dbSet;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Application>();
        }
        public async Task<Application> GetByIdAsync(long id) => await _dbSet.FindAsync(id);

        public async Task<ICollection<Application>> GetAllAsync() => await _dbSet
        .Include(a => a.User)  // Инклудим пользователя
        .Include(a => a.Event) // Инклудим мероприятие
        .AsNoTracking()
        .ToListAsync();

        public async Task<bool> CreateEntityAsync(Application model)
        {
            var existingApplication = await _context.Application
                .FirstOrDefaultAsync(a => a.UserId == model.UserId && a.EventId == model.EventId);

            var user = await _context.Application.FirstOrDefaultAsync(e => e.UserId == model.UserId);
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.eventId == model.EventId);

            /*if (@event == null) return false;*/ // Если мероприятия нет, ничего не делаем

            if (existingApplication != null)
            {
                existingApplication.Places += 1; // Обновляем запись, а не создаем новую

                if(@event.Places == 0)
                {
                    throw new InvalidOperationException("У мероприятия закончились места");
                }
                @event.Places -= 1; // Уменьшаем места в мероприятии

                _context.Update(user);
                _context.Update(@event);

                return await SaveAsync();
            }

            // Если заявки ещё нет, создаём её
            await _dbSet.AddAsync(model);
            model.Places += 1;
            @event.Places -= 1;

            _context.Update(@event);

            return await SaveAsync();
        }

        public async Task<bool> UpdateEntityAsync(Application model)
        {
            _dbSet.Update(model);

            return await SaveAsync();
        }

        public async Task<bool> DeleteEntity(Application model)
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
