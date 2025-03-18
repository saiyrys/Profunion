using FluentEmail.Core;
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
using profunion.Domain.Persistance;

namespace profunion.Applications.Services.EmailService
{
    public class EmailEventSender : IEmailEventSender
    {
        private readonly IConfiguration _configuration;

        private readonly IUserRepository _userRepository;

        private readonly IEventRepository _eventRepository;

        public EmailEventSender(IConfiguration configuration, IUserRepository userRepository ,IEventRepository eventRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }
        public async Task SendUserEventLink(long userId, string eventId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Пользователь не найден");

            var @event = await _eventRepository.GetByIdAsync(eventId);
            if (@event== null)
                throw new ArgumentException("Мероприятие не найдено");

            string emailBody = LoadHtmlTemplate("EventInvitation.html", new Dictionary<string, string>
            {
                { "{{title}}", @event.title },
                { "{{Link}}", @event.link }
            });

            string subject = $"Мероприятие {@event.title}";

            EmailSenderSettings sender = new EmailSenderSettings(_configuration);

            await sender.SendMessage(user.email, subject, emailBody);
        }

        private string LoadHtmlTemplate(string templateName, Dictionary<string, string> replacements)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", templateName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Шаблон {templateName} не найден.");

            var template = File.ReadAllText(path);

            foreach (var item in replacements)
            {
                template = template.Replace(item.Key, item.Value);
            }

            return template;
        }
    }
}
