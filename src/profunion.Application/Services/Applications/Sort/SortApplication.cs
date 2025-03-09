using profunion.Applications.Interface.IApplications;
using profunion.Shared.Dto.Application;

namespace profunion.Applications.Services.Applications.Sort
{
    public class SortApplication : ISortApplication
    {
        public IEnumerable<GetApplicationDto> SortObject(IEnumerable<GetApplicationDto> application, SortStateApplication? sort)
        {
            switch (sort)
            {
                case SortStateApplication.PlacesAsc:
                    application = application.OrderBy(a => a.places);
                    break;
                case SortStateApplication.PlacesDesc:
                    application = application.OrderByDescending(a => a.places);
                    break;
                case SortStateApplication.DateAsc:
                    application = application.OrderBy(a => a.createdAt);
                    break;
                case SortStateApplication.DateDesc:
                    application = application.OrderByDescending(a => a.createdAt);
                    break;
                default:
                    application = application.OrderBy(a => a.places);
                    break;
            }

            return application.ToList();
        }
    }
}
