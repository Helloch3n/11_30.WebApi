using ActiveDirectoryService.Application.Interface;
using ActiveDirectoryService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();

            // 可继续注册其他仓储或服务
            return services;
        }
    }
}
