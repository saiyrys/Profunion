using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IUser
{
    public interface ISortUser
    {
        IEnumerable<GetUserDto> SortObject(IEnumerable<GetUserDto> users, SortStateUser? sort);
    }
}
