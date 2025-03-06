using profunion.Domain.Models.UserModels;
using profunion.Shared.Common.dto;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IUser
{
    public interface IUserService
    {
        Task<(IEnumerable<GetUserDto> Users, int TotalPage)> GetUsersByAdmin(int page, UserQueryDto query, SortStateUser sort);

        Task<GetUserDto> GetUserById(long userId);

        Task<bool> UpdateUsers(long userId, UpdateUserDto updateUser);

        Task<bool> DeleteUser(long userId);
    }
}
