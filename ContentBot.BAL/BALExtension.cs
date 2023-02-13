using ContentBot.BAL.Services;
using ContentBot.BAL.Services.Interfaces;
using ContentBot.DAL;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentBot.BAL
{
    public static class BALExtension
    {
        public static IServiceCollection AddBLResolver(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddMemoryCache();

            return services;
        }

    }
}
