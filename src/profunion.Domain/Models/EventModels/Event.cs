using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using profunion.Domain.Models.UploadModel;
using System.ComponentModel.DataAnnotations;

namespace profunion.Domain.Models.EventModels
{
    public class Event
    {
        public Event()
        {
            eventId = Guid.NewGuid().ToString();
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
            totalPlaces = Places;
        }
        public string eventId { get; set; }
        [MaxLength(25)]
        public string title { get; set; }
        public string description { get; set; }
        public string organizer { get; set; }
        public DateTime date { get; set; }
        public string link { get; set; }
        public int totalPlaces { get; set; }
        public int Places { get; set; }
        public bool isActive { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public virtual ICollection<Categories> Categories { get; set; }
        public virtual ICollection<EventCategories> EventCategories { get; set; }

        public virtual ICollection<Uploads> Uploads { get; set; }
        public virtual ICollection<EventUploads> EventUploads { get; set; }

    }

    
}
