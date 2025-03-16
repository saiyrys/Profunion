using Microsoft.EntityFrameworkCore;
using profunion.Applications.Interface.IEvents;
using profunion.Applications.Interface.IEvents.IService;
using profunion.Applications.Mapper;
using profunion.Applications.Services.Events;
using profunion.Applications.Services.Events.Sort;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Infrastructure.Persistance.Repository;
using AutoMapper;
using profunion.Applications.Interface.IAuth;
using profunion.Applications.Services.Auth;
using profunion.Domain.Factories.Users;
using profunion.Infrastructure.Factories;
using profunion.Infrastructure.SomeService.HashPassword;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using profunion.Applications.Interface.ICategory;
using profunion.Applications.Services.Category;
using profunion.Applications.Interface.IApplications;
using profunion.Applications.Services.Applications;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Applications.Interface.IFiles;
using profunion.Applications.Services.Media;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Users;
using profunion.Applications.Interface.IUser;
using profunion.Applications.Services.Users;
using profunion.Infrastructure.SomeService.UpdateEntity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using profunion.Applications.Services.Users.Sort;
using profunion.Applications.Interface.INews;
using profunion.Applications.Services.Newses;
using profunion.Applications.Services.Newses.Sort;
using profunion.Applications.Services.Applications.Sort;
using profunion.API;
using profunion.API.Background;
using profunion.Applications.Interface.IEmailService;
using profunion.Applications.Services.EmailService;
using profunion.Domain.Constants;
using profunion.Applications.Services.Auth.ResetPassword;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Зависимости категорий
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
// Зависимости мероприятий
builder.Services.AddScoped<IEventReaderService, EventReaderService>();
builder.Services.AddScoped<IEventWriterService, EventWriterService>();
builder.Services.AddScoped<ISearchable, GetEventDto>();
builder.Services.AddScoped<IPagination, Pagination>();
builder.Services.AddScoped<ISortAction, SortAction>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
// Зависимости на Авторизацию
builder.Services.AddScoped(typeof(IControl<>), typeof(Control<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UserFactory, CreateUserFactory>();
builder.Services.AddScoped<TokenGeneration>();
builder.Services.AddScoped<IHashingPassword, HashingPassword>();
builder.Services.AddHttpContextAccessor();
// Зависимости пользователя
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISortUser, SortUser>();
builder.Services.AddScoped<ISearchable, GetUserDto>();
builder.Services.AddControllers();

//зависимости заявок
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<ISortApplication, SortApplication>();

// Зависимости для Новостей
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ISortNews, SortNews>();

//Зависимости для Медиа
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSingleton<AddFileQueue>();
builder.Services.AddHostedService<AddFileService>();


// Зависимости для почты
builder.Services.AddScoped<IEmailAuthSender, EmailAuthSender>();
builder.Services.AddScoped<IResetPasswordMethods, ResetPasswordMethods>();
builder.Services.AddScoped<IEmailResetSender, EmailResetSender>();
builder.Services.AddSingleton<AuthCodeCache>();
builder.Services.AddSingleton<CacheResetToken>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy
                .WithOrigins("https://localhost:5173", "http://localhost:5173", "http://localhost:5173", "http://profunions.ru", "https://profunions.ru", "https://www.profunions.ru")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IUpdateMethods, UpdateMethods>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secretKey = Encoding.UTF8.GetBytes(Auth_Constants.JWT_SECRET_KEY);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Валидация времени жизни токена
        };
        options.SaveToken = true;

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["accessToken"];

                if (string.IsNullOrEmpty(token) && context.Request.Headers.ContainsKey("authorization"))
                {
                    var authHeader = context.Request.Headers["authorization"].ToString();
                    Console.WriteLine($"Authorization header: {authHeader}"); // Лог для проверки

                    if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }

                /*Console.WriteLine($"Final token: {token}"); // Лог для проверки*/

                context.Token = token;
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

/*builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // Разрешаем прослушивание на всех интерфейсах
});*/

var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.UseMiddleware<ExceptionHandlingMiddleware>();
/*app.UseHttpsRedirection();*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

public partial class Program { }