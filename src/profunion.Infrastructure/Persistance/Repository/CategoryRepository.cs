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
        private readonly DbSet<Categories> _dbSet;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Categories>();
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
                var rangeToDelete = await _context.Events.Include(e => e.EventCategories)
                    .Where(e => e.EventCategories.Any(ec => ec.CategoriesId == categoryId)).ToListAsync(); 
                
                var EventCategoriesToDelete = await _context.EventCategories.Where(ec => ec.CategoriesId == categoryId).ToListAsync(); 
                _context.EventCategories.RemoveRange(EventCategoriesToDelete);

                await SaveAsync();
    
                foreach(var delete in rangeToDelete)
                {
                    if (!await _context.EventCategories.AnyAsync(ec => ec.eventId == delete.eventId))
                    {
                        _context.Events.Remove(delete);
                    }
                }

                return true;
            }
            catch(Exception ex)
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
