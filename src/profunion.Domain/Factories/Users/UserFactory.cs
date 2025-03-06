using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using profunion.Domain.Models.UserModels;

namespace profunion.Domain.Factories.Users
{
    public abstract class UserFactory
    {
        public abstract Task<User> CreateUser(string userName, string firstName, string middleName, string lastName, string email, string password, string role);
        /*public abstract Task<UserDto> CreateUserForSendByEmail(string userName, string password, string role);*/

        public virtual long GenerateId()
        {
            Random random = new Random();
            return ((long)(uint)random.Next(int.MinValue, int.MaxValue)) << 32 | (uint)random.Next(int.MinValue, int.MaxValue);
        }
    }
}
