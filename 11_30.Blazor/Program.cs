using _11_30.Blazor;
using _11_30.Blazor.Components;
using _11_30.Blazor.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 获取 BaseAddress 配置
var baseAddress = builder.Configuration["BaseAddress"];
//添加HTTP请求Base URL
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress=new Uri(baseAddress!)
});
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddFluentUIComponents();
builder.Services.AddScoped<QueryApiService>();
builder.Services.AddScoped<EJiaApiService>();
builder.Services.AddScoped<ActiveDirectoryApiService>();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
        // 可选：设置长连接保活时间
    });

builder.Services.Configure<HubOptions>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);  // 默认值是 30 秒
    options.KeepAliveInterval = TimeSpan.FromMinutes(2);       // 默认 15 秒
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
