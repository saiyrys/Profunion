using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profunion.Applications.Interface.IEmailService;
using profunion.Shared.Dto.Events;

namespace profunion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSender : Controller
    {
        private readonly IEmailAuthSender _emailSender;

        public EmailSender(IEmailAuthSender emailSender)
        {
            _emailSender = emailSender; 
        }


        /*[HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendMessage(long userId, string eventId)
        {
            var result = _emailSender.SendLinkForEmail(userId, eventId);

           *//* if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }*//*

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Мероприятие успешно создан");
        }*/

    }
}
