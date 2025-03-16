using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Application
{
    public class CreateApplicationDto
    {
        public string eventId { get; set; }

        public string userId { get; set; }

        public int places { get; set; }

    }
}
