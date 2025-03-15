using AutoMapper;
using profunion.Applications.Interface.IAuth;
using profunion.Applications.Interface.IEmailService;
using profunion.Applications.Services.EmailService;
using profunion.Domain.Factories.Users;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.SomeService.HashPassword;
using profunion.Shared.Dto.Auth;
using profunion.Shared.Dto.Users;
using SendGrid.Helpers.Errors.Model;


namespace profunion.Applications.Services.Auth
{
    public class AuthService : IAuthService, IPasswordService
    {
        private readonly IControl<User> _control;

        private readonly TokenGeneration _generateToken;

        private readonly IHashingPassword _hashingPassword;
        private readonly IMapper _mapper;

        private readonly UserFactory _userFactory;

        private readonly IUserRepository _userRepository;

        private readonly IEmailSender _emailSender;

        public AuthService(IControl<User> control,
            TokenGeneration generateToken, IHashingPassword hashingPassword,
            IMapper mapper, UserFactory userFactory, IUserRepository userRepository,
             IEmailSender emailSender)
        {
            _control = control;
            _generateToken = generateToken;
            _hashingPassword = hashingPassword;
            _mapper = mapper;

            _userFactory = userFactory;

            _userRepository = userRepository;

            _emailSender = emailSender;

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

        public async Task<LoginResponseDto> LoginEmail(string email, string code)
        {
            var user = await _control.FindByEmailAsync(email);

            if (user == null)
                throw new ArgumentException("Email не найден в системе");

            /*await _emailSender.SendAuthorizationCode(email);*/

            string cacheCode = await AuthCodeCache.GetCode(email);

            if (code != cacheCode)
                throw new UnauthorizedAccessException("Неверный код авторизации");

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

            var userProfileDto = _mapper.Map<UserInfoDto>(user);


            var newToken = await _generateToken.GenerateToken(userDto);

            return new LoginResponseDto
            {
                AccessToken = newToken.Item1,
                User = userProfileDto
            };
        }

        private async Task<bool> ValidatePassword(string loginPassword, string dbPassword, string salt)
            => await _hashingPassword.VerifyPassword(loginPassword, dbPassword, salt);

        public async Task<bool> ChangePassword(string token, long userId, string password)
        {
            var currentUser = await _control.FindByTokenAsync(token);

            if (currentUser.role == "ADMIN" || currentUser.userId == userId)
            {
                var user = await _userRepository.GetByIdAsync(userId);

                var (newPass, salt) = await _hashingPassword.HashPassword(password);
                user.password = newPass;
                user.salt = Convert.ToBase64String(salt);

                await _userRepository.UpdateAsync(user);

                return true;
            }

            throw new UnauthorizedAccessException();
        }

        
    }
}
