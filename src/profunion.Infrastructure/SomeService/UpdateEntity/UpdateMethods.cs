using Microsoft.EntityFrameworkCore;
using profunion.Infrastructure.Data;
using profunion.Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.SomeService.UpdateEntity
{
    public class UpdateMethods : IUpdateMethods
    {
        private readonly ApplicationDbContext _context;

        public UpdateMethods(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateEntity<TEntity, TUpdateDto, Tkey>(Tkey id, TUpdateDto updateDto)
            where TEntity : class
            where TUpdateDto : class
        {
            var updateModel = await _context.Set<TEntity>().FindAsync(id);

            if (updateModel == null)
                throw new ArgumentException($"{typeof(TEntity).Name} does not exist");

            var modelType = typeof(TEntity);
            var updateDtoType = typeof(TUpdateDto);

            foreach (var property in updateDtoType.GetProperties())
            {
                var newValue = property.GetValue(updateDto);
                if (newValue != null)
                {
                    var entityProperty = modelType.GetProperty(property.Name);
                    if (entityProperty != null && entityProperty.CanWrite)
                    {
                        /*if (newValue is int intValue && intValue == 0) // Проверяем `int`, включая `0`
                        {
                            entityProperty.SetValue(updateModel, intValue);
                        }
                        else if (newValue is bool boolValue) // Обновляем `bool`
                        {
                            entityProperty.SetValue(updateModel, boolValue);
                        }
                        else if (newValue != null) // Остальные типы
                        {
                            entityProperty.SetValue(updateModel, newValue);
                        }
*/
                        entityProperty.SetValue(updateModel, newValue);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
