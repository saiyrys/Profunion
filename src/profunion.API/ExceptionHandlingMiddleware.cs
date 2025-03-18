namespace profunion.API
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly bool _showDetailedErrors;  // флаг для вывода подробных ошибок

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _showDetailedErrors = configuration.GetValue<bool>("ShowDetailedErrors"); // Настройка из конфигурации
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);  // Переходим к следующему компоненту pipeline
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "Ошибка обработки запроса");

                // Устанавливаем статус ошибки
                context.Response.StatusCode = 500;

                var responses = new
                {
                    response = new
                    {
                        data = new
                        {
                            message = _showDetailedErrors ? ex.Message : "Внутренняя ошибка сервера"
                        }
                    }
                };

                // Возвращаем ошибку с данными
                await context.Response.WriteAsJsonAsync(responses);
            }
        }
    }
}
