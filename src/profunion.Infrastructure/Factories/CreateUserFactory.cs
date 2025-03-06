using profunion.Domain.Factories.Users;
using profunion.Domain.Models.UserModels;
using profunion.Infrastructure.SomeService.HashPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace profunion.Infrastructure.Factories
{
    public class CreateUserFactory : UserFactory
    {
        public override async Task<User> CreateUser(string userName, string firstName, string middleName, string lastName, string email, string password, string role)
        {
            HashingPassword hashing = new();

            var (hashedPassword, salt) = await hashing.HashPassword(password);

            return new User
            {
                userId = GenerateId(),
                userName = userName,
                firstName = firstName,
                middleName = middleName,
                lastName = lastName,
                email = email,
                password = hashedPassword,
                salt = Convert.ToBase64String(salt),
                role = role
            };
        }
    }

    /*public override async Task<UserDto> CreateUserForSendByEmail(string userName, string password, string role)
        {
            return new UserDto
            {
                userId = GenerationUniqueId(),
                userName = userName,
                password = password,
               *//* role = CheckUserRole(role)*//*
            };
        }*/
}
