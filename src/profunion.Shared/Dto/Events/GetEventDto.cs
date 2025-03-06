using profunion.Domain.Models.EventModels;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Dto.Category;
using profunion.Shared.Dto.Uploads;
using System.Data;

namespace profunion.Shared.Dto.Events
{
    public class GetEventDto : ISearchable
    {
        public string eventId { get; set; }
        public List<GetUploadsDto> images { get; set; }
        public string? title { get; set; }
        public string organizer { get; set; }
        public string? description { get; set; }
        public string eventDate { get; set; }
        public string link { get; set; }
        public ICollection<CategoriesDto> categories { get; set; }
        public bool isActive { get; set; }
        public bool status { get; set; }
        public int places { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }

        public IEnumerable<string> GetSearchableFields()
        {
            return new List<string>
            {
                title,
                organizer,
            };
        }
    }
}
