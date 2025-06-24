using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.EventModels
{
    public class EventCategories
    {
        public string CategoriesId { get; set; }
        public Categories Categories { get; set; }

        public string eventId { get; set; }
        public Event Event { get; set; }
    }
}
