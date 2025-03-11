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
        public GetEventDto @event { get; set; }

        public int takePlaces { get; set; }
    }
}
