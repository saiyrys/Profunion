using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using profunion.Domain.Models.MailModels;

namespace profunion.Applications.Services.EmailService
{
    public class EmailSenderSettings
    {
        private readonly IConfiguration _configuration;

        public EmailSenderSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(string email, string subject, string body)
        {
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
                Subject = $"{subject}",
                Body = $"{body}",
                IsBodyHtml = true
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
    }
}
