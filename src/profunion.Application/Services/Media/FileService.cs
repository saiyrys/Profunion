using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IFiles;
using profunion.Domain.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Media
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private string _uploadPath;

        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration, IFileRepository fileRepository)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
            _uploadPath = string.Empty;
        }

        public async Task<(string id, string filename, string url)> WriteFile(IFormFile file, string fileType, CancellationToken cancellation)
        {
            // определение пути хранения
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Определяем путь к папке uploads внутри bin
            var sharedFolder = Path.Combine(directory, "uploads");

            var uploadFolder = fileType switch
            {
                "Event" => "EventUploads", // Для фото ивента
                "News" => "NewsUploads", // Для фото новости
                _ => throw new ArgumentException("Unsupported file type"), // если передали неподдерживаемый тип
            };

            _uploadPath = Path.Combine(sharedFolder, uploadFolder);

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            // получаем расширение файла
            var extension = Path.GetExtension(file.FileName);
            // Генерация уникального имени
            var filename = Guid.NewGuid().ToString() + extension;
            // генерация пути сохранения
            var filePath = Path.Combine(_uploadPath, filename);

            // Создание и запись в уникальное место.
            using (var stream = new FileStream(filePath, FileMode.Create)) // открывает файл по пути  FileMode.Create - если файл есть, то перезапишется, если нет, то создасться
            {
                // Копирует содержимое файла и записывает
                await file.CopyToAsync(stream);
            }
            //Запись данных файла в БД
            var fileId = await _fileRepository.AddFileAsync(filename, filePath);

            var baseUrl = fileType switch
            {
                "Event" => _configuration["EventUrl"], // URL для фото ивентов
                "News" => _configuration["NewsUrl"], // URL для фото новостей
                _ => throw new ArgumentException("Unsupported file type"),
            };

            string fileUrl = $"{baseUrl}{filename}";

            Uri url = new Uri(fileUrl);

            return (fileId, filename, fileUrl);
        }

        public Task<string> OpenFile(string fileName)
        {
            var file = _fileRepository.GetFile(fileName);

            return file;
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var file = _fileRepository.DeleteFile(fileName);

            return true;
        }

        public async Task<bool> DeleteEventFile(string eventId)
        {
            var file = _fileRepository.DeleteEventFile(eventId);

            return true;
        }

       /* public async Task<bool> DeleteUserFile(long userId, string fileName)
        {
            var file = _fileRepository.DeleteUserFile(userId, fileName);

            return true;
        }*/
    }
}
