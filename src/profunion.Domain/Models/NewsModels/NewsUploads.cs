using profunion.Domain.Models.UploadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.NewsModels
{
    public class NewsUploads
    {
        public string newsId { get; set; }
        public News News { get; set; }

        public string fileId { get; set; }
        public Uploads Uploads { get; set; }
    }
}
