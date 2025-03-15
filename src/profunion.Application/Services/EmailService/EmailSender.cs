using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IEmailService;
using profunion.Domain.Models.MailModels;
using profunion.Domain.Persistance;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace profunion.Applications.Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        IConfiguration _configuration;


        public EmailSender(IUserRepository userRepository, IEventRepository eventRepository, IConfiguration configuration)
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


            var mailSettings = GetMailSettings(domain);

            using var smtpClient = new SmtpClient(mailSettings.SmtpServer)
            {
                Port = mailSettings.Port,
                Credentials = new NetworkCredential(mailSettings.Username, mailSettings.Password),
                EnableSsl = mailSettings.EnableSsl
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

        public string GenerateAuthCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString(); // 6-значный код
            return code;
        }

        private MailSettings GetMailSettings(string domain)
        {
            switch (domain.ToLower())
            {
                case "gmail.com":
                    return _configuration.GetSection("MailSettings:Gmail").Get<MailSettings>();
                case "mail.ru":
                    return _configuration.GetSection("MailSettings:MailRu").Get<MailSettings>();
                case "yandex.ru":
                    return _configuration.GetSection("MailSettings:YandexRu").Get<MailSettings>();
                default:
                    throw new InvalidOperationException("Не поддерживаемый почтовый сервис");
            }
        }
    }
}
