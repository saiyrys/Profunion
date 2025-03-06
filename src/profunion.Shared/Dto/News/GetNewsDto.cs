using profunion.Shared.Common.Interfaces;
using profunion.Shared.Dto.Uploads;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.News
{
    public class GetNewsDto : ISearchable
    {
        public string newsId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public List<GetUploadsDto> images { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public int views { get; set; }

        public IEnumerable<string> GetSearchableFields()
        {
            return new List<string>
            {
                title,
                description,
                content,
            };
        }
    }
}

