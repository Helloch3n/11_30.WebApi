using _11_30.Domain;
using _11_30.Domain.Repositories;
using _11_30.Domain.Services;
using _11_30.Infrastructure.External.Excel;
using _11_30.Infrastructure.External.Selenium;
using _11_30.Infrastructure.External.SOA;
using _11_30.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace _11_30.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IQuestionBankDomainService, QuestionBankDomainService>();
            services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
            services.AddScoped<IGeneralQueryTypeRepository, GeneralQueryTypeRepository>();
            services.AddScoped<ISeleniumService, SeleniumService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IErpService, ErpService>();
            services.AddScoped<IExcelService, ExcelService>();
            //services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            // 可继续注册其他仓储或服务
            return services;
        }
    }
}
