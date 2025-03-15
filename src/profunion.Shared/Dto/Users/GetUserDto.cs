using profunion.Domain.Models.UserModels;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Dto.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Shared.Dto.Users
{
    public class GetUserDto : ISearchable
    {
        public string userName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public IEnumerable<string> GetSearchableFields()
        {
            return new List<string>
            {
                userName,
                firstName,
                middleName,
                lastName,
                email,
                role
            };
        }
    }
}
