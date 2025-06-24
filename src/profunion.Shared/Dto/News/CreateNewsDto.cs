using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.News
{
    public class CreateNewsDto
    {
        public string title { get; set; }
        public string content { get; set; }
        public List<string> imagesId { get; set; }
    }
}
