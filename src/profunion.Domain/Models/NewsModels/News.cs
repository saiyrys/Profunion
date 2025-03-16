using profunion.Domain.Models.UploadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.NewsModels
{
    public class News
    {
        public News()
        {
            newsId = Guid.NewGuid().ToString();
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
        public string newsId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int views { get; set; }

        public virtual ICollection<Uploads> Uploads { get; set; }
        public virtual ICollection<NewsUploads> NewsUploads { get; set; }

    }
}
