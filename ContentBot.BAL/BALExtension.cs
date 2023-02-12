using ContentBot.BAL.Services;
using ContentBot.BAL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL
{
    public static class BALExtension
    {
        public static IServiceCollection AddBLResolver(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IAccountService, AccountService>();

            return services;
        }

    }
}
