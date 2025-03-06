using profunion.Domain.Models.EventModels;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Application
{
    public class GetUserApplicationDto
    {
        public string id { get; set; }

        public long userId { get; set; }

        public string eventId { get; set; }


        public GetUserInfo user { get; set; }
        public GetEventInfo @event { get; set; }

        public int places { get; set; }
    }
}
