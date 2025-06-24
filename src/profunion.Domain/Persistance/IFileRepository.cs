using Microsoft.AspNetCore.Http;
using profunion.Domain.Models.UploadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Domain.Persistance
{
    public interface IFileRepository
    {
        Task<Uploads> GetFiles();
        Task<string> GetFile(string fileId);
        /*        Task<Uploads> GetFileByName(string fileName);*/
        Task<string> AddFileAsync(string fileId, string filename, string filePath);
        /*  Task DeleteUserFile(long userId, string fileId);*/

        Task<bool> DeleteFile(string fileName);
        Task DeleteNewsFile(string newsId);
        Task DeleteEventFile(string eventId/*, string fileId*/);

    }
}
