using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using profunion.Applications.Interface.IEvents;
using profunion.Applications.Interface.IEvents.IService;
using profunion.Applications.Interface.IFiles;
using profunion.Domain.Persistance;
using profunion.Shared.Dto.Events;

namespace profunion.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventReaderService _readerService;
        private readonly IEventWriterService _writerService;
        private readonly IEventRepository _eventRepository;
        private readonly IFileService _fileService;

        public EventController(IEventReaderService readerService, IEventWriterService writerService, IFileService fileService, IEventRepository eventRepository)
        {
            _readerService = readerService;
            _writerService = writerService;
            _fileService = fileService;
            _eventRepository = eventRepository;
        }

        // Пути на работу с ивентами
        [HttpGet]
     /*   [Authorize]*/
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetEvents(int page, [FromQuery] EventQueryDto query, SortState sort)
        {
            var (events, totalPages) = await _readerService.GetEvents(page, query, sort);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Items = events, countPage = totalPages });
        }

        [HttpGet("{eventId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEvent(string eventId)
        {
            var events = await _readerService.GetEventsByID(eventId);

            return Ok(events);
        }

        // Генерация отчета по ивентам
        [HttpGet("report")]
       /* [Authorize(Roles = "ADMIN, MODER")]*/
        public async Task<IActionResult> GetEventsStatusReport()
        {
            var events = await _eventRepository.GetAllAsync(); // Получаем список мероприятий

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets.Add("Мероприятия");

                worksheet.Cells[1, 1].Value = "Название мероприятия";
                worksheet.Cells[1, 2].Value = "Дата и время проведения";
                worksheet.Cells[1, 3].Value = "Организатор мероприятия";
                worksheet.Cells[1, 4].Value = "Общее количество мест";
                worksheet.Cells[1, 5].Value = "Количество сделанных заявок";
                worksheet.Cells[1, 6].Value = "Количество оставшихся мест";

                int row = 2;
                foreach (var ev in events)
                {
                    worksheet.Cells[row, 1].Value = ev.title;
                    worksheet.Cells[row, 2].Value = ev.date.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[row, 3].Value = ev.organizer;
                    worksheet.Cells[row, 4].Value = ev.totalPlaces;
                    worksheet.Cells[row, 5].Formula = $"=D{row}-F{row}";
                    worksheet.Cells[row, 6].Value = ev.Places;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"event_report_{DateTime.Now:yyyy_MM_dd_HH_mm}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateImageEvents([FromBody] CreateEventDto eventsCreate, CancellationToken cancellation)
        {
            if (eventsCreate == null)
            {
                return BadRequest();
            }

            var result = await _writerService.CreateEvents(eventsCreate, cancellation);

            if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Мероприятие успешно создан");
        }

        [HttpPatch("{eventId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateEvents(string eventId, [FromBody] UpdateEventDto updateEvent, CancellationToken cancellation)
        {
            if (updateEvent == null)
            {
                return BadRequest("Invalid data.");
            }

            var eventToUpdate = await _writerService.UpdateEvents(eventId, updateEvent, cancellation);

            if (eventToUpdate)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("{eventId}")]
        [Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEvents(string eventId, CancellationToken cancellation)
        {
            var result = await _writerService.DeleteEvents(eventId, cancellation);

            if (!result)
            {
                return NotFound("Мероприятие не найдено");
            }

            return Ok("Мероприятие успешно удалено");
        }



        // Работа с фотографиями ивентов
        [HttpPost("image")]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateImageEvents(IFormFile image, CancellationToken cancellation)
        {
            if (image == null)
            {
                return BadRequest();
            }

            (string Id,string filename, string Url) = await _fileService.WriteFile(image, "Event", cancellation);

            var result = new
            {
                id = Id,
                name = filename,
                url = Url
            };

            if (result == null)
            {
                return StatusCode(500, "Ошибка при создании медиа");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Task.Delay(1000);

            return Ok(result);
        }

        [HttpDelete("image/{fileName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteImage(string fileName)
        {
            var filePath = await _fileService.DeleteFile(fileName);

            if (filePath == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(true);
        }
    }
}
