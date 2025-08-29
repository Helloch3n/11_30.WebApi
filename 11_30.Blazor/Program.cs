using _11_30.Blazor;
using _11_30.Blazor.Components;
using _11_30.Blazor.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ��ȡ BaseAddress ����
var baseAddress = builder.Configuration["BaseAddress"];
//���HTTP����Base URL
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
        // ��ѡ�����ó����ӱ���ʱ��
    });

builder.Services.Configure<HubOptions>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);  // Ĭ��ֵ�� 30 ��
    options.KeepAliveInterval = TimeSpan.FromMinutes(2);       // Ĭ�� 15 ��
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
