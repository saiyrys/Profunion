using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.SomeService.HashPassword
{
    public interface IHashingPassword
    {
        Task<(string HashedPassword, byte[] Salt)> HashPassword(string password);

        public byte[] GenerateSalt();

        Task<bool> VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
