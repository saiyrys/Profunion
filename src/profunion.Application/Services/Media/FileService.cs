using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using profunion.Applications.Interface.IFiles;
using profunion.Domain.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using profunion.Shared.Dto.Uploads;
using profunion.Domain.Models.UploadModel;

namespace profunion.Applications.Services.Media
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly AddFileQueue _fileQueue;
        private string _uploadPath;
        private readonly ILogger<FileService> _logger;

        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration, IFileRepository fileRepository, AddFileQueue fileQueue, ILogger<FileService> logger)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
            _fileQueue = fileQueue;
            _logger = logger;
            _uploadPath = string.Empty;
        }

        public async Task<(string id, string filename, string url)> WriteFile(IFormFile file, string fileType, CancellationToken cancellation)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл не может быть пустым");
            }
            try
            {
                // определение пути хранения
                var directory = Path.Combine(AppContext.BaseDirectory, "uploads");

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
                var fileId = Guid.NewGuid().ToString();

                var filename = Guid.NewGuid().ToString() + extension;
                // генерация пути сохранения
                var filePath = Path.Combine(_uploadPath, filename);

                // Создание и запись в уникальное место.
                await using var fileStream = file.OpenReadStream();
                await using var output = new FileStream(filePath, FileMode.Create);
                await fileStream.CopyToAsync(output, cancellation);
                //Запись данных файла в БД
                _fileQueue.Enqueue(new Uploads { id = fileId, fileName = filename, filePath = filePath});

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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке файла");
                throw new Exception("Ошибка при загрузке файла. Попробуйте позже.");
            }
        }

        public Task<string> OpenFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Файл не может быть пустым");
            }

            var file = _fileRepository.GetFile(fileName);

            return file;
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentException("Файл не может быть пустым");
            }
            var file = _fileRepository.DeleteFile(fileName);

            return true;
        }

       /* public async Task<bool> DeleteUserFile(long userId, string fileName)
        {
            var file = _fileRepository.DeleteUserFile(userId, fileName);

            return true;
        }*/
    }
}
