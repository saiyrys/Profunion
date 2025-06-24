using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.News;

namespace profunion.Applications.Services.Newses.Sort
{
    public class SortViewModel
    {
        public SortStateNews AlphabeticSort { get; }
        public SortStateNews ViewsAsc { get; }
        public SortStateNews DateSort { get; }
        public SortStateNews CurrentSort { get; }
        public SortStateNews Current { get; }

        public SortViewModel(SortStateNews sortOrder)
        {
            AlphabeticSort = sortOrder == SortStateNews.AlphabeticAsc ? SortStateNews.AlphabeticDesc : SortStateNews.AlphabeticAsc;

            ViewsAsc = sortOrder == SortStateNews.ViewsAsc ? SortStateNews.ViewsDesc : SortStateNews.ViewsAsc;

            DateSort = sortOrder == SortStateNews.DateAsc ? SortStateNews.DateDesc : SortStateNews.DateAsc;

            CurrentSort = sortOrder = SortStateNews.Current;

            Current = sortOrder;
        }
    }
}
