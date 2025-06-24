using Microsoft.EntityFrameworkCore;
using profunion.Shared.Common.Interfaces;



namespace profunion.Shared.Common.Service
{
    public static class Search<T> 
    {
        public static async Task<IEnumerable<T>> SearchEntities<T>(IEnumerable<T> query, string search) where T : ISearchable
        {
            if (string.IsNullOrEmpty(search))
                return new List<T>();

            query.AsQueryable();

            search = search.ToLower();

            return query.Where(entity => entity.GetSearchableFields().Any(field => field.ToLower().Contains(search))).ToList();
        }
    }
}
