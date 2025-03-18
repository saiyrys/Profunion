using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IEmailService;
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
    public class EmailResetSender : IEmailResetSender
    {
        IConfiguration _configuration;

        public EmailResetSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailResetPasswordAsync(string email, string body)
        {
            string subject = "Сброс пароля";

            EmailSenderSettings sender = new EmailSenderSettings(_configuration);

            await sender.SendMessage(email, subject, body); 
        }
    }
}
