using profunion.Shared.Dto.Application;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IApplications
{
    public interface ISortApplication
    {
        IEnumerable<GetApplicationDto> SortObject(IEnumerable<GetApplicationDto> application, SortStateApplication? sort);
    }
}
