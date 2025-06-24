using AutoMapper;
using profunion.Applications.Interface.IApplications;
using profunion.Applications.Interface.IEmailService;
using profunion.Applications.Interface.IEvents.IService;
using profunion.Domain.Models.ApplicationModels;
using profunion.Domain.Models.NewsModels;
using profunion.Domain.Persistance;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Application;

namespace profunion.Applications.Services.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEventReaderService _eventService;
        private readonly IMapper _mapper;
        private readonly IPagination _pagination;
        private readonly ISortApplication _sortApplication;
        private readonly IEmailEventSender _linkSender;

        public ApplicationService(IApplicationRepository applicationRepository, IEventReaderService eventService, IMapper mapper,IPagination pagination, ISortApplication sortApplication, IEmailEventSender linkSender)
        {
            _applicationRepository = applicationRepository;
            _eventService = eventService;
            _mapper = mapper;
            _pagination = pagination;
            _sortApplication = sortApplication;
            _linkSender = linkSender;
        }
        public async Task<bool> CreateApplication(CreateApplicationDto createApplication)
        {
            if (createApplication == null)
            {
                throw new ArgumentException("поля не монут быть пустым");
            }

            var application = _mapper.Map<Application>(createApplication);

            if (!await _applicationRepository.CreateEntityAsync(application, createApplication.places))
            {
                throw new InvalidOperationException();
            }

            long userId = long.Parse(createApplication.userId);

            await _linkSender.SendUserEventLink(userId, createApplication.eventId);

            return true;
        }

        public async Task<(IEnumerable<GetApplicationDto>, int TotalPages)> GetApplication(int page, ApplicationQueryDto query, SortStateApplication sort)
        {
            var applications = await _applicationRepository.GetAllAsync();
            
            var applicationsDto = _mapper.Map<IEnumerable<GetApplicationDto>>(applications);

            if (!string.IsNullOrEmpty(query.search))
            {
                applicationsDto = await Search<GetApplicationDto>.SearchEntities(applicationsDto, query.search);
            }

            if (sort != SortStateApplication.Current)
            {
                applicationsDto = _sortApplication.SortObject(applicationsDto, sort);
            }

            if (query.created_at_start != null || query.created_at_end != null)
            {
                applicationsDto = applicationsDto.Where(n =>
                    (!string.IsNullOrEmpty(query.created_at_start) ? n.createdAt.Date >= DateTime.Parse(query.created_at_start).Date : true) &&
                    (!string.IsNullOrEmpty(query.created_at_end) ? n.createdAt.Date <= DateTime.Parse(query.created_at_end).Date : true) 
                ).ToList();
            }

            var paginate = await _pagination.Paginate(applicationsDto, page);

            applicationsDto = paginate.Items;
            int totalPages = paginate.TotalPages;

            return (applicationsDto, totalPages);
        }

        public async Task<IEnumerable<GetApplicationDto>> GetApplicationForReport()
        {
            var applications = await _applicationRepository.GetAllAsync();

            var applicationsDto = _mapper.Map<IEnumerable<GetApplicationDto>>(applications);

            return applicationsDto;
        }

        public async Task<(IEnumerable<GetUserApplicationDto>, int TotalPages)> GetUserApplication(long userId, int page)
        {
            var applications = await _applicationRepository.GetAllAsync();

            var userApplications = applications.Where(u => u.UserId == userId);

            var events = await _eventService.GetFullEventData(); // Загружаем мероприятия

            var applicationsDto = userApplications.Select(app => new GetUserApplicationDto
            {
                @event = events.FirstOrDefault(e => e.eventId == app.EventId), // Присоединяем мероприятие
                takePlaces = app.Places // Берем количество мест
            });

            var paginateItem = await _pagination.Paginate(applicationsDto, page);

            return (paginateItem.Items, paginateItem.TotalPages);
        }
    }
}
