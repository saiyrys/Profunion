using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.NewsModels;
using profunion.Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.UploadModel
{
    public class Uploads
    {
        public string id { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }

       /* public virtual ICollection<UserUploads>? UserUploads { get; set; }*/

        public virtual ICollection<EventUploads>? EventUploads { get; set; }

        public virtual ICollection<NewsUploads>? NewsUploads { get; set; }
    }
}
