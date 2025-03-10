using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.IFiles
{
    public interface IFileService
    {
        Task<(string id, string filename, string url)> WriteFile(IFormFile file,string fileType, CancellationToken cancellation);

        Task<string> OpenFile(string fileName);

        Task<bool> DeleteFile(string fileName);
        /* Task<bool> DeleteUserFile(long userId, string fileName);*/
    }
}
