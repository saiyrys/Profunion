using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.News
{
    public class UpdateNewsDto
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public List<string>? imagesId { get; set; }
        public string? content { get; set; }
    }
}
