using ActiveDirectoryService.Application;
using ActiveDirectoryService.Infrastructure;
using EventBus.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.Configure<ActiveDirectoryOptions>(
    builder.Configuration.GetSection("ActiveDirectoryOptions"));

// 1️ 注册 EventBus
builder.Services.AddEventBusRabbitMQ(options =>
{
    options.HostName = "localhost"; // RabbitMQ 地址
    options.Port = 5672;
    options.UserName = "guest";
    options.Password = "guest";
    options.ExchangeName = "global_events_exchange";   // 所有微服务共用
    options.QueueName = "ad_webapi_queue";    // 每个微服务不同的队列名
    options.DurableExchange = true;
});

// 2 注册 Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AD:";
});



// 配置 Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()                 // 最低日志级别
    .WriteTo.File(
        path: "Logs/log-.txt",                 // 日志文件路径
        rollingInterval: RollingInterval.Day,  // 每天创建新文件
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

// 替换默认日志系统
builder.Host.UseSerilog();


var app = builder.Build();


// 使用示例
var logger = app.Logger;
logger.LogInformation("应用启动成功");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
