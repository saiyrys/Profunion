using AutoMapper;
using profunion.Applications.Interface.IAuth;
using profunion.Domain.Factories.Users;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.SomeService.HashPassword;
using profunion.Shared.Dto.Auth;
using profunion.Shared.Dto.Users;
using SendGrid.Helpers.Errors.Model;


namespace profunion.Applications.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IControl<User> _control;

        private readonly TokenGeneration _generateToken;

        private readonly IHashingPassword _hashingPassword;
        private readonly IMapper _mapper;

        private readonly UserFactory _userFactory;

        private readonly IUserRepository _userRepository;
 
        public AuthService(IControl<User> control,
            TokenGeneration generateToken, IHashingPassword hashingPassword,
            IMapper mapper, UserFactory userFactory, IUserRepository userRepository)
        {
            _control = control;
            _generateToken = generateToken;
            _hashingPassword = hashingPassword;
            _mapper = mapper;

            _userFactory = userFactory;

            _userRepository = userRepository;

        }

        public async Task<LoginResponseDto> Login(LoginUserDto loginUser)
        {
            var user = await _control.FindByNameAsync(loginUser.userName);
            
            if (user == null || !await ValidatePassword(loginUser.password, user.password, user.salt))
                throw new BadRequestException("Неправильный логин или пароль");
            
            var userDto = _mapper.Map<UserInfoDto>(user);

            var token = await _generateToken.GenerateToken(userDto);

            return new LoginResponseDto
            {
                AccessToken = token.Item1,
                User = userDto
            };

        }

        public async Task<bool> Registration(RegistrationDto registration)
        {
            var user = await _control.FindByNameAsync(registration.userName);

            if (user != null)
                throw new BadRequestException();

            var createUser = await _userFactory.CreateUser(
                registration.userName,
                registration.firstName,
                registration.middleName,
                registration.lastName,
                registration.email,
                registration.password,
                registration.role
                );

            var userMap = _mapper.Map<User>(createUser);

            if(!await _userRepository.CreateAsync(userMap))
                throw new BadRequestException("Что то пошло не так при сохранении данных");

            return true;
        }

        public async Task<UserInfoDto> GetUser(string token)
        {
            var accessToken = await _control.VerifyByTokenAsync();

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("Token is unregister");

            var user = await _control.FindByTokenAsync(token);

            if (user == null)
                throw new ArgumentNullException("User not found");

            var userDto = _mapper.Map<UserInfoDto>(user);

            return userDto;
        }

        public async Task<LoginResponseDto> GetNewTokens(string refreshToken)
        {
            var user = await _control.FindByTokenAsync(refreshToken);

            var userDto = _mapper.Map<UserInfoDto>(user);

            var newToken = await _generateToken.GenerateToken(userDto);

            return new LoginResponseDto
            {
                AccessToken = newToken.Item1,
                User = userDto
            };
        }

        private async Task<bool> ValidatePassword(string loginPassword, string dbPassword, string salt)
            => await _hashingPassword.VerifyPassword(loginPassword, dbPassword, salt);
    }
}
