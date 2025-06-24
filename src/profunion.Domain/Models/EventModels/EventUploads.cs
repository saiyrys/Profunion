using profunion.Domain.Models.UploadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.EventModels
{
    public class EventUploads
    {
        public string eventId { get; set; }
        public Event Event { get; set; }

        public string fileId { get; set; }
        public Uploads Uploads { get; set; }
    }
}
