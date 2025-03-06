using profunion.Shared.Dto.Application;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IApplications
{
    public interface IApplicationService
    {
        Task<(IEnumerable<GetApplicationDto>, int TotalPages)> GetApplication(int page, ApplicationQueryDto query, SortStateApplication sort);

        Task<IEnumerable<GetApplicationDto>> GetApplicationForReport();

        Task<bool> CreateApplication(CreateApplicationDto createApplication);

        Task<(IEnumerable<GetApplicationDto>, int TotalPages)> GetUserApplication(long userId, int page);
    }
}
