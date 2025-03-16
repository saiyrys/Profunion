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

            var domain = string.Join(".", email.Split('@')[1].Split('.').TakeLast(2));

            var mailSettings = _configuration.GetSection("MailSettings:MailRu").Get<MailSettings>();

            using var smtpClient = new SmtpClient(mailSettings.SmtpServer)
            {
                Port = mailSettings.Port,
                Credentials = new NetworkCredential(mailSettings.Username, mailSettings.Password),
                EnableSsl = mailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailSettings.SenderEmail, mailSettings.SenderName),
                Subject = "Код авторизации для приложения",
                Body = $"Ваш код авторизации: {code}",
                IsBodyHtml = false
            };

            mailMessage.To.Add(email);

            try
            {
                // Отправляем письмо
                await smtpClient.SendMailAsync(mailMessage);
            }

            catch (Exception ex)
            {
                // Логируем ошибку и сообщаем о неудаче
                Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                throw new InvalidOperationException("Ошибка при отправке письма", ex);
            }
        }

        private string GenerateAuthCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString(); // 6-значный код
            return code;
        }
    }
}
