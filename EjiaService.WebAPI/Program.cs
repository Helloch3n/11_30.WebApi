using EjiaService.Application.Events;
using EjiaService.Application.Handler;
using EventBus.Abstractions;
using EventBus.Extensions;
using EjiaService.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 注册 EventBus
builder.Services.AddEventBusRabbitMQ(options =>
{
    options.HostName = "localhost"; // RabbitMQ 地址
    options.Port = 5672;
    options.UserName = "guest";
    options.Password = "guest";
    options.ExchangeName = "global_events_exchange";   // 所有微服务共用
    options.QueueName = "ejia_webapi_queue";    // 每个微服务不同的队列名
    options.DurableExchange = true;
});

builder.Services.AddApplication();



var app = builder.Build();



// 在应用启动时订阅事件
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    eventBus.AddApplicationEventBusSubscriptions(); // 调用 Application 层的扩展方法
}

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
