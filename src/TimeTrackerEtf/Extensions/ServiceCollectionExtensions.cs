﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace TimeTrackerEtf.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtBearerAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        var tokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = configuration["Tokens:Issuer"],
                            ValidAudience = configuration["Tokens:Issuer"],
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(configuration["Tokens:Key"]))
                        };

                        options.TokenValidationParameters = tokenValidationParameters;
                    });
        }
    }
}
