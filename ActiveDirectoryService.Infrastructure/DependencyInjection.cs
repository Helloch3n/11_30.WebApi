using ActiveDirectoryService.Infrastructure.ActiveDirectory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IActiveDirectoryClient, ActiveDirectoryClient>();

            // 可继续注册其他仓储或服务
            return services;
        }
    }
}
