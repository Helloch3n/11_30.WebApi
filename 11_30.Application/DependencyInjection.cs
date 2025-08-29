using _11_30.Application.Interface;
using _11_30.Application.Services;
using _11_30.Domain;
using _11_30.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace _11_30.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
            services.AddScoped<IQuestionBankAppService, QuestionBankAppService>();
            services.AddScoped<IGeneralQueryAppService, GeneralQueryAppService>();
            services.AddScoped<ILxAppService, LxAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEJiaAppService, EJiaAppService>();
            return services;
        }
    }
}
