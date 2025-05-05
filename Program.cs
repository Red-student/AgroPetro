using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OpenEcologyApp.Data;
using Microsoft.EntityFrameworkCore;
using OpenEcologyApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Регистрируем контекст базы данных и указываем SQLite и путь к файлу базы данных "app.db"
builder.Services.AddDbContext<EcologyDbContext>(options =>
    options.UseSqlite("Data Source=app.db")); // Здесь создаётся файл app.db при первом запуске

// Добавляем фоновую службу для автоматического обновления данных
builder.Services.AddHostedService<GrainDataUpdateService>();

// Регистрируем сервис импорта XLS-файлов
builder.Services.AddScoped<XlsImporter>();

// Настройка политики CORS: разрешаем доступ с любого домена
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Настраиваем HttpClient с базовым адресом для API-запросов
builder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri("https://localhost:7290/");
});

// Регистрируем пользовательские сервисы
builder.Services.AddScoped<JsonDataService>();
builder.Services.AddScoped<GrainHarvestService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StatisticsService>();

var app = builder.Build();

// Инициализация базы данных при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcologyDbContext>();

        // Проверка подключения к базе данных
        if (!context.Database.CanConnect())
        {
            // Если базы нет — создаём и импортируем данные
            context.Database.EnsureCreated();

            // Добавляем начального администратора
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                Password = "admin123", // В реальном приложении пароль должен быть хеширован
                IsAdmin = true
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // Импортируем данные о зерновых
            var importer = new XlsImporter(context);
            await importer.ImportGrainDataAsync();
        }

        // Логируем количество загруженных записей
        var count = context.GrainHarvests.Count();
        Console.WriteLine($"База данных инициализирована. Добавлено {count} записей.");
    }
    catch (Exception ex)
    {
        // Обработка ошибок и логирование
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Показываем страницу ошибки в проде
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection();     // Перенаправление на HTTPS
app.UseStaticFiles();          // Обслуживание статических файлов (например, CSS, JS)
app.UseRouting();              // Включаем маршрутизацию
app.UseCors();                 // Применяем CORS

// Настройка конечных точек
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();              // Подключаем контроллеры API
    endpoints.MapBlazorHub();                // Включаем поддержку SignalR (Blazor)
    endpoints.MapFallbackToPage("/_Host");   // Обработка всех других запросов через _Host.cshtml
});

// Повторная инициализация данных, может быть полезна при старте
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcologyDbContext>();
        context.Database.EnsureCreated(); // Убедимся, что база существует

        var harvestService = services.GetRequiredService<GrainHarvestService>();
        await harvestService.InitializeDataAsync(); // Инициализация пользовательских данных
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}

// Запуск приложения
app.Run();
