using profunion.Shared.Common.Interfaces;

namespace profunion.Shared.Dto.Category
{
    public class CategoriesDto : ISearchable
    {
        public string id { get; set; }
        public string name { get; set; }

        public IEnumerable<string> GetSearchableFields()
        {
            return new List<string>
            {
                name
            };
        }
    }
}
