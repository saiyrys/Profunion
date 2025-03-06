using profunion.Applications.Interface.IEvents;
using profunion.Applications.Interface.IUser;
using profunion.Shared.Dto.Users;

namespace profunion.Applications.Services.Users.Sort
{
    public class SortUser : ISortUser
    {
        public IEnumerable<GetUserDto> SortObject(IEnumerable<GetUserDto> users, SortStateUser? sort)
        {
            switch (sort)
            {
                case SortStateUser.AlphabeticAsc:
                    users = users.OrderBy(u => u.lastName);
                    break;
                case SortStateUser.AlphabeticDesc:
                    users = users.OrderByDescending(u => u.lastName);
                    break;
                default:
                    users = users.OrderBy(u => u.userName);
                    break;
            }

            return users.ToList();
        }
    }
}
