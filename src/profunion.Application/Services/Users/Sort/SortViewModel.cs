using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Users.Sort
{
    public class SortViewModel
    {
        public SortStateUser AlphabeticSort { get; }
        public SortStateUser CurrentSort { get; }
        public SortStateUser Current { get; }

        public SortViewModel(SortStateUser sortOrder)
        {
            AlphabeticSort = sortOrder == SortStateUser.AlphabeticAsc ? SortStateUser.AlphabeticDesc : SortStateUser.AlphabeticAsc;

            CurrentSort = sortOrder = SortStateUser.Current;

            Current = sortOrder;
        }
    }
}
