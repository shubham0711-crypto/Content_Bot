﻿using ContentBot.DAL.Context;
using ContentBot.DAL.Entities;
using ContentBot.DAL.Repository;
using ContentBot.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.DAL
{
    public static class DALExtension
    {
        public static void AddDALResolver(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContentBotDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
                options.EnableSensitiveDataLogging();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddIdentity<ApplicationUser, Roles>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ContentBotDbContext>();


            services.AddTransient<IAccountRepository, AccountRepository>();
        }

    }
}