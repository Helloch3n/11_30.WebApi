var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 添加 YARP 服务
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


//设置跨域
var vueUrl = builder.Configuration["VueUrl"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        policy.WithOrigins(vueUrl)          // Vue 前端地址
              .AllowAnyHeader()            // 允许所有请求头
              .AllowAnyMethod()            // 允许 GET/POST/PUT/DELETE 等
              .AllowCredentials();        // 如果前端发送 Cookie
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 CORS
app.UseCors("AllowVue");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



// 使用 YARP 中间件
app.MapReverseProxy();

app.Run();
