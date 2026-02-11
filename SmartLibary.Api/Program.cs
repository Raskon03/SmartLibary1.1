using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Application.Services;
using SmartLibrary.Application.Validators;
using SmartLibrary.Infrastructure;
using SmartLibrary.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog Setup (Логване)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Configuration (Връзваме настройките от appsettings.json с класа MongoDbSettings)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// 3. Dependency Injection (Свързваме Интерфейсите с Реализациите)
// "Когато някой поиска IBookRepository, дай му BookRepository"
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILendingService, LendingService>();

// 4. Validators (FluentValidation)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

// 5. Health Checks (Проверка за здраве)
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => HealthCheckResult.Healthy());

// 6. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- PIPELINE (Редът на изпълнение на заявките) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Логва всяка заявка
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health"); // Ендпойнт за проверка дали работи

app.Run();