using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IEmailService;
using profunion.Domain.Models.MailModels;
using profunion.Domain.Persistance;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace profunion.Applications.Services.EmailService
{
    public class EmailAuthSender : IEmailAuthSender
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        IConfiguration _configuration;


        public EmailAuthSender(IUserRepository userRepository, IEventRepository eventRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;

            _eventRepository = eventRepository;

            _configuration = configuration;
        }

        public async Task SendAuthorizationCode(string email)
        {
            string code = GenerateAuthCode();

            AuthCodeCache.UpdateCache(email, code);

            string subject = "Код авторизации для приложения";

            string body = $"Ваш код авторизации: {code}";

            EmailSenderSettings sender = new EmailSenderSettings(_configuration);

            await sender.SendMessage(email, subject, body);
        }

        private string GenerateAuthCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString(); // 6-значный код
            return code;
        }
    }
}
