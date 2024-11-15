﻿using HackathonAPI.Configurations;
using HackathonAPI.Contracts;
using HackathonAPI.Data;
using HackathonAPI.Interfaces;
using HackathonAPI.Models;
using HackathonAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAny", opts => opts.AllowAnyHeader().AllowAnyMethod()
                 .WithOrigins("http://localhost:4200", "https://localhost:4200"));
            });
          
            services.AddAutoMapper(typeof(AutomapperConfig));
           
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            
            services.AddIdentityCore<User>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
