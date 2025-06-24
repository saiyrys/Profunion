
using profunion.Applications.Services.Media;
using profunion.Domain.Persistance;

namespace profunion.API.Background
{
    public class AddFileService : BackgroundService
    {
        private readonly AddFileQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AddFileService> _logger;

        public AddFileService(AddFileQueue queue, IServiceScopeFactory scopeFactory, ILogger<AddFileService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var file = await _queue.DequeueAsync(stoppingToken);
                if (file == null) continue;

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fileRepository = scope.ServiceProvider.GetRequiredService<IFileRepository>();

                    await fileRepository.AddFileAsync(file.id, file.fileName, file.filePath);
                    _logger.LogInformation("Файл {FileName} сохранен в БД", file.fileName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при записи файла {FileName} в БД", file.fileName);
                }
            }
        }
    }
}
