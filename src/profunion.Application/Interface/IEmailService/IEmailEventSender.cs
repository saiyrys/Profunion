using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IEmailService
{
    public interface IEmailEventSender
    {
        Task SendUserEventLink(long userId, string eventId);
    }
}
