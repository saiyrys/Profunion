using profunion.Shared.Dto.Application;

namespace profunion.Applications.Services.Applications.Sort
{
    public class SortViewModel
    {
        public SortStateApplication PlacesSort { get; }
        public SortStateApplication DateSort { get; }
        public SortStateApplication CurrentSort { get; }
        public SortStateApplication Current { get; }

        public SortViewModel(SortStateApplication sortOrder)
        {
            PlacesSort = sortOrder == SortStateApplication.PlacesAsc ? SortStateApplication.PlacesDesc : SortStateApplication.PlacesAsc;

            DateSort = sortOrder == SortStateApplication.DateAsc ? SortStateApplication.DateDesc : SortStateApplication.DateAsc;

            CurrentSort = sortOrder = SortStateApplication.Current;

            Current = sortOrder;
        }
    }
}
