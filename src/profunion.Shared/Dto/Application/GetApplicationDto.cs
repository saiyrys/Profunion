using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UserModels;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Application
{
    public class GetApplicationDto : ISearchable
    {
        public string? id { get; set; }

        public GetUserInfo? user { get; set; }
        public GetEventInfo? @event { get; set; }

        public int? places { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public IEnumerable<string> GetSearchableFields()
        {
            return new List<string>
            {
                user.firstName,
                user.lastName,
                @event.title,
            };
        }

    }
}
