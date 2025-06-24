using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.MailModels
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}
