using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IEvents;
using profunion.Applications.Interface.IEvents.IService;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Category;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Uploads;

namespace profunion.Applications.Services.Events
{
    public class EventReaderService : IEventReaderService
    {
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISortAction _sortAction;
        private readonly IPagination _pagination;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public EventReaderService(IEventRepository repository, IMapper mapper, ISortAction sortAction, IPagination pagination, IConfiguration configuration, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _sortAction = sortAction;
            _pagination = pagination;
            _configuration = configuration;
            _context = context;
        }
        public async Task<(IEnumerable<GetEventDto> Events, int TotalPages)> GetEvents(int page, EventQueryDto query, SortState sort)
        {
            int pageSize = 12;

            var events = await GetFullEventData();

            if (!string.IsNullOrEmpty(query.search))
            {
                events = await Search<GetEventDto>.SearchEntities(events, query.search);
            }

            if (sort != SortState.Current)
            {
                events = _sortAction.SortObject(events, sort);
            }

            if(query.date_start != null || query.date_end != null || query.time_start != null || query.time_end != null)
            {
                events = events.Where(e =>
                    (!string.IsNullOrEmpty(query.date_start) ? e.date.Date >= DateTime.Parse(query.date_start).Date : true) &&
                    (!string.IsNullOrEmpty(query.date_end) ? e.date.Date <= DateTime.Parse(query.date_end).Date : true) &&
                    (!string.IsNullOrEmpty(query.time_start) ? e.date.TimeOfDay >= DateTime.Parse(query.time_start).TimeOfDay : true) &&
                    (!string.IsNullOrEmpty(query.time_end) ? e.date.TimeOfDay <= DateTime.Parse(query.time_end).TimeOfDay : true)
                ).ToList();
            }

            var paginationItem = await _pagination.Paginate(events.ToList(), page);

            events = paginationItem.Items;
            int totalPages = paginationItem.TotalPages;

            return (events, totalPages);
        }

        public async Task<GetEventDto> GetEventsByID(string eventId)
        {
            var events = await GetFullEventData();

            var @event = events.FirstOrDefault(e => e.eventId == eventId);

            var @eventMap = _mapper.Map<GetEventDto>(@event);

            return @eventMap;
        }

        private async Task<IEnumerable<GetEventDto>> GetFullEventData()
        {
            var baseUrl = _configuration["EventUrl"];
            var events = await _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Categories)
                .Include(e => e.EventUploads)
                    .ThenInclude(eu => eu.Uploads)
                 .Select(e => new GetEventDto
                 {
                     eventId = e.eventId,
                     title = e.title,
                     description = e.description,
                     organizer = e.organizer,
                     date = e.date,
                     link = e.link,
                     places = e.Places,
                     isActive = e.isActive,
                     status = e.status,
                     createdAt = e.createdAt.ToString("yyyy-MM-dd"),
                     updatedAt = e.updatedAt.ToString("yyyy-MM-dd"),
                     images = e.EventUploads.Select(eu => new GetUploadsDto
                     {
                         id = eu.fileId,
                         name = $"{_context.Uploads.FirstOrDefault(u => u.id == eu.fileId).fileName}",
                         Url = $"{baseUrl}{_context.Uploads.FirstOrDefault(u => u.id == eu.fileId).fileName}"
                     }).ToList(),
                     categories = e.EventCategories.Select(ec => new CategoriesDto { id = ec.Categories.Id, name = ec.Categories.name}).ToList()
                 }).ToListAsync();

            return events;
        }
    }
}
