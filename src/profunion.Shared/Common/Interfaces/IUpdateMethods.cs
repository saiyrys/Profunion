using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Common.Interfaces
{
    public interface IUpdateMethods
    {
        Task<bool> UpdateEntity<TEntity, TUpdateDto, Tkey>(Tkey id, TUpdateDto updateDto)
           where TEntity : class
           where TUpdateDto : class;
    }
}
