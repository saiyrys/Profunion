using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.UploadModel;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.Persistance.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;
        private string _uploadPath;
        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Uploads> GetFiles()
        {
            return await _context.Uploads.OrderBy(u => u.id).FirstOrDefaultAsync();
        }
        public async Task<string> GetFile(string fileName)
        {
            var upload = await _context.Uploads.FirstOrDefaultAsync(u => u.fileName == fileName);

            if (upload != null)
            {
                return upload.filePath;
            }

            return null;

        }
        /* public Task<Uploads> GetFileByName(string fileName)
         {
             throw new NotImplementedException();
         }*/

        public async Task<string> AddFileAsync(string fileId, string filename, string filePath)
        {
            var uploadedFile = new Uploads
            {
                id = fileId,
                fileName = filename,
                filePath = filePath
            };

            _context.Uploads.Add(uploadedFile);
            await _context.SaveChangesAsync();

           
            return (uploadedFile.id);
        } 
        /*public async Task DeleteUserFile(long userId, string fileId)
        {
            var userUpload = await _context.UserUploads
                .FirstOrDefaultAsync(uu => uu.userId == userId && uu.fileId == fileId);

            var upload = await _context.Uploads.FirstOrDefaultAsync(u => u.id == fileId);

            var filePath = upload.filePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (userUpload != null)
            {
                _context.UserUploads.RemoveRange(userUpload);
            }
            _context.Uploads.Remove(upload);

            await _context.SaveChangesAsync();
        }
*/
        public async Task DeleteNewsFile(string newsId)
        {
            var newsUpload = await _context.NewsUploads
                .FirstOrDefaultAsync(nu => nu.newsId == newsId);

            if (newsUpload == null)
            {
                return; // Файл не найден, выходим
            }

            var fileId = newsUpload.fileId;

            var upload = await _context.Uploads.FirstOrDefaultAsync(u => u.id == fileId);
            if (upload == null)
            {
                return; // Файл уже удален, выходим
            }

            var filePath = upload.filePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.NewsUploads.RemoveRange(newsUpload);
            _context.Uploads.Remove(upload);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var upload = _context.Uploads.FirstOrDefault(u => u.fileName == fileName);
            if (upload == null) return false;  // Если файл не найден, выходим

            var filePath = upload.filePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Получаем ID файла
            var fileId = upload.id;

            // Удаляем связи в промежуточных таблицах
            _context.NewsUploads.RemoveRange(_context.NewsUploads.Where(nu => nu.fileId == fileId));
            _context.EventUploads.RemoveRange(_context.EventUploads.Where(eu => eu.fileId == fileId));

            // Удаляем сам файл из Uploads
            _context.Uploads.Remove(upload);

            return await SaveUploads();
        }


        public async Task DeleteEventFile(string eventId)
        {
            var eventUpload = await _context.EventUploads
                .FirstOrDefaultAsync(eu => eu.eventId == eventId);

            if (eventUpload == null)
            {
                return; // Файл не найден, выходим
            }

            var fileId = eventUpload.fileId;

            var upload = await _context.Uploads.FirstOrDefaultAsync(u => u.id == fileId);
            if (upload == null)
            {
                return; // Файл уже удален, выходим
            }

            var filePath = upload.filePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.EventUploads.Remove(eventUpload);
            _context.Uploads.Remove(upload);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveUploads()
        {
            await _context.SaveChangesAsync();

            return true;
        }

       
    }
}
