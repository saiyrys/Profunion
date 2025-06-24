using profunion.Domain.Models.UploadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Models.UserModels
{
    public class User
    {
        public User()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

        public long userId { get; set; }
        public string? userName { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public string email { get; set; }
        public string? password { get; set; }
        public string salt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string role { get; set; }


/*        public virtual ICollection<UserUploads> UserUploads { get; set; }*/
    }
}
