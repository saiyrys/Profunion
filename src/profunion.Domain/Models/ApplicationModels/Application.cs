using System.ComponentModel.DataAnnotations.Schema;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;

namespace profunion.Domain.Models.ApplicationModels
{
    public class Application
    {
        public Application()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string Id { get; set; }

        public string EventId { get; set; }

        public long UserId { get; set; }

        public int Places { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
