using profunion.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Events.Sort
{
    public class SortViewModel
    {
        public SortState AlphabeticSort { get; }
        public SortState EventDateSort { get; }
        public SortState PlacesSort { get; }
        public SortState CurrentSort { get; }
        public SortState Current { get; }



        public SortViewModel(SortState sortOrder)
        {
            AlphabeticSort = sortOrder == SortState.AlphabeticAsc ? SortState.AlphabeticDesc : SortState.AlphabeticAsc;

            EventDateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;

            PlacesSort = sortOrder == SortState.PlacesAsc ? SortState.PlacesDesc : SortState.PlacesAsc;

            CurrentSort = sortOrder = SortState.Current;

            Current = sortOrder;
        }
    }
}
