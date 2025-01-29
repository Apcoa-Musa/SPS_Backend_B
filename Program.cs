using GarageQueueDownload.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);

// L�gg till RabbitMQ-tj�nst
builder.Services.AddSingleton<RabbitMqService>(sp =>
{
    var configuration = builder.Configuration;
    var hostName = configuration["RabbitMQ:HostName"] ?? throw new ArgumentNullException("RabbitMQ:HostName saknas");
    var queueName = configuration["RabbitMQ:QueueName"] ?? "QueueUpdates";
    var userName = configuration["RabbitMQ:UserName"] ?? "guest";
    var password = configuration["RabbitMQ:Password"] ?? "guest";
    var port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");

    return new RabbitMqService(hostName, queueName, port, userName, password);
});

// L�gg till Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = builder.Configuration["Redis:ConnectionString"];
    if (string.IsNullOrEmpty(redisConnection))
    {
        throw new InvalidOperationException("Redis-anslutningsstr�ngen saknas i appsettings.json");
    }

    return ConnectionMultiplexer.Connect(redisConnection);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// L�gg till HttpClient-tj�nst f�r CarParksApiService
builder.Services.AddHttpClient<CarParksApiService>(client =>
{
    var baseUrl = builder.Configuration["CarParksApi:BaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("BaseUrl saknas i appsettings.json f�r CarParksApi.");
    }
    client.BaseAddress = new Uri(baseUrl);

    var apiKey = builder.Configuration["CarParksApi:ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
    }
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// L�gg till Controllers
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseCors("AllowLocalhost4200"); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
