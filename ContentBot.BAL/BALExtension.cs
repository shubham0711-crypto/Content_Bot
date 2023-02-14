using ContentBot.BAL.Services;
using ContentBot.BAL.Services.Interfaces;
using ContentBot.DAL;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace ContentBot.BAL
{
    public static class BALExtension
    {
        public static IServiceCollection AddBLResolver(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddOpenAIService(options => options.ApiKey = configuration["OpenAiServiceOptions:ApiKey"]);          

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IImageGenerationService, ImageGenerationService>();
            services.AddMemoryCache();

            return services;
        }

    }
}
