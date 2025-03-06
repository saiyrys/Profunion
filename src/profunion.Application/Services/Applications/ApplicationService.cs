using AutoMapper;
using profunion.Applications.Interface.IApplications;
using profunion.Domain.Models.ApplicationModels;
using profunion.Domain.Persistance;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Application;

namespace profunion.Applications.Services.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IPagination _pagination;
        private readonly ISortApplication _sortApplication;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper,IPagination pagination, ISortApplication sortApplication)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _pagination = pagination;
            _sortApplication = sortApplication;
        }
        public async Task<bool> CreateApplication(CreateApplicationDto createApplication)
        {
            var application = _mapper.Map<Application>(createApplication);

            if (!await _applicationRepository.CreateEntityAsync(application))
            {
                throw new InvalidOperationException();
            }

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

        public async Task<(IEnumerable<GetApplicationDto>, int TotalPages)> GetUserApplication(long userId, int page)
        {
            var applications = await _applicationRepository.GetAllAsync();

            var userApplications = applications.Where(u => u.UserId == userId);

            var applicationsDto = _mapper.Map<IEnumerable<GetApplicationDto>>(userApplications);

            var paginateItem = await _pagination.Paginate(applicationsDto, page);
            applicationsDto = paginateItem.Items;

            int totalPages = paginateItem.TotalPages;

            return (applicationsDto, totalPages);
        }
    }
}
