using Microsoft.EntityFrameworkCore;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly DbSet<Categories> _dbSet;

        public CategoryRepository(ApplicationDbContext context, IFileRepository fileRepository)
        {
            _context = context;
            _dbSet = context.Set<Categories>();
            _fileRepository = fileRepository;
        }

        public async Task<Categories> GetByIdAsync(string id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<Categories>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<bool> CreateEntityAsync(Categories model)
        {
            await _dbSet.AddAsync(model);

            return await SaveAsync();
        }

        public async Task<bool> UpdateEntityAsync(Categories model)
        {
            _dbSet.Update(model);

            return await SaveAsync();
        }

        public async Task<bool> DeleteEntity(Categories model)
        {
            await RemoveRange(model.Id);

            _dbSet.Remove(model);

            return await SaveAsync();
        }

        public async Task<bool> RemoveRange(string categoryId)
        {
            try
            {
                // 1. Получаем все события, связанные с категорией
                var rangeToDelete = await _context.Events.Include(e => e.EventCategories)
                    .Where(e => e.EventCategories.Any(ec => ec.CategoriesId == categoryId))
                    .ToListAsync();

                // 2. Удаляем все связи между событиями и категорией в промежуточной таблице
                var EventCategoriesToDelete = await _context.EventCategories
                    .Where(ec => ec.CategoriesId == categoryId)
                    .ToListAsync();

                _context.EventCategories.RemoveRange(EventCategoriesToDelete);

                // 3. Сохраняем изменения в EventCategories
                await SaveAsync();

                // 4. Для каждого события проверяем, если оно больше не связано с другими категориями
                foreach (var delete in rangeToDelete)
                {
                    // Проверяем, если у события осталась только одна категория (которую мы удаляем)
                    if (!await _context.EventCategories.AnyAsync(ec => ec.eventId == delete.eventId))
                    {
                        // 5. Удаляем файлы, если событие больше не связано с категориями
                        await _fileRepository.DeleteEventFile(delete.eventId);

                        // 6. Удаляем заявки для этого события из таблицы Application
                        var applicationEntries = await _context.Application
                            .Where(a => a.EventId == delete.eventId)
                            .ToListAsync();

                        _context.Application.RemoveRange(applicationEntries);

                        // 7. Удаляем само событие из базы данных
                        _context.Events.Remove(delete);
                    }
                }

                // 8. Сохраняем изменения после удаления событий, файлов и заявок
                await SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
