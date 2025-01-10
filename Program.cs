using System;
using GarageQueueUpload.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Läs in konfiguration från appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Lägg till HttpClient för CarParksApiService
builder.Services.AddHttpClient<CarParksApiService>(client =>
{
    var baseUrl = builder.Configuration["CarParksApi:BaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("BaseUrl saknas i appsettings.json för CarParksApi.");
    }
    client.BaseAddress = new Uri(baseUrl);
    Console.WriteLine($"CarParksApi BaseAddress: {client.BaseAddress}");

    var apiKey = builder.Configuration["CarParksApi:ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
    }
});

// Lägg till Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = builder.Configuration["Redis:ConnectionString"];
    if (string.IsNullOrEmpty(redisConnection))
    {
        throw new InvalidOperationException("Redis-anslutningssträngen saknas i appsettings.json");
    }

    try
    {
        var multiplexer = ConnectionMultiplexer.Connect(redisConnection);
        Console.WriteLine("Redis connected successfully.");
        return multiplexer;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to connect to Redis: {ex.Message}");
        throw;
    }
});

// Lägg till RabbitMQ
builder.Services.AddSingleton<RabbitMqService>(sp =>
{
    try
    {
        var configuration = builder.Configuration;
        var config = new RabbitMqConfig
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
            QueueName = configuration["RabbitMQ:QueueName"] ?? "QueueUpdates"
        };

        return new RabbitMqService(config);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to initialize RabbitMQ: {ex.Message}");
        throw;
    }
});

// Lägg till CarParksApiService
builder.Services.AddSingleton<CarParksApiService>();

// Lägg till CORS-stöd
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Tillåt endast denna origin
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Lägg till Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lägg till controllers
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware för CORS
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    app.Run();
    Console.WriteLine("Application started successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Application failed to start: {ex.Message}");
}
