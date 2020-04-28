﻿using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;
using Zdimk.Services;
using Zdimk.Services.Configuration;

namespace Zdimk.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddPictureService(this IServiceCollection services)
        {
            services.AddSingleton<IPictureService, PictureService>();
        }

        public static void AddAuthorizationBundle<TUser>(this IServiceCollection services)
            where TUser: IdentityUser
        {
            services.AddAuthorization()
                .AddIdentity<TUser, IdentityRole>()
                .AddTokenProvider<JwtSecutiryTokenProvider<TUser>>("jwt")
                .AddEntityFrameworkStores<ZdimkDbContext>();
        }

        public static void AddAuthenticationBundle(this IServiceCollection services, Action<JwtTokenOptions> config)
        {
            var  options = new JwtTokenOptions();
            services.Configure(config);
            config.Invoke(options);

            var symmetricKey = new SymmetricSecurityKey(options.PrivateKey);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.AccessTokenAudience,
                        IssuerSigningKey = symmetricKey
                    };
                });
        }
    }
}