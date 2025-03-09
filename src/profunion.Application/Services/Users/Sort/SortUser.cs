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
                case SortStateUser.CreatedAsc:
                    users = users.OrderBy(u => u.createdAt);
                    break;
                case SortStateUser.CreatedDesc:
                    users = users.OrderByDescending(u => u.createdAt);
                    break;
                case SortStateUser.UpdatedAsc:
                    users = users.OrderBy(u => u.updatedAt);
                    break;
                case SortStateUser.UpdatedDesc:
                    users = users.OrderByDescending(u => u.updatedAt);
                    break;
                default:
                    users = users.OrderBy(u => u.userName);
                    break;
            }

            return users.ToList();
        }
    }
}
