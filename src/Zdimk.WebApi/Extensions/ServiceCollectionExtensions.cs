using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Zdimk.Application.Exceptions;
using Zdimk.Application.Implementations;
using Zdimk.Application.Implementations.Configuration;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;

namespace Zdimk.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(paramName: nameof(services));

            services.AddSwaggerGen(setupAction: c =>
            {
                c.SwaggerDoc(name: "v1", info: new OpenApiInfo
                {
                    Title = "Zdimk api v1",
                    Version = "v1"
                });
            });

            return services;
        }

        public static void AddPictureService(this IServiceCollection services)
        {
            services.AddSingleton<IPictureService, PictureService>();
        }

        public static void AddAuthorizationBundle<TUser, TKey>(this IServiceCollection services)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            services.AddAuthorization()
                .AddIdentity<TUser, IdentityRole<TKey>>()
                .AddTokenProvider<JwtSecutiryTokenProvider<TUser, TKey>>("jwt")
                .AddEntityFrameworkStores<MainDbContext>();
        }

        public static void AddAuthenticationBundle(this IServiceCollection services, Action<JwtTokenOptions> config)
        {
            var options = new JwtTokenOptions();
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
                        ValidateLifetime = true,

                        ValidIssuer = options.Issuer,
                        ValidAudience = options.AccessTokenAudience,
                        IssuerSigningKey = symmetricKey
                    };
                });
        }

        public static void AddHellangProblemDetails(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(paramName: nameof(services));

            services.AddProblemDetails(configure: options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();

                options.MapToStatusCode<RecordNotFoundException>(statusCode: StatusCodes.Status404NotFound);

                options.MapToStatusCode<NotImplementedException>(statusCode: StatusCodes.Status501NotImplemented);

                options.MapToStatusCode<HttpRequestException>(statusCode: StatusCodes.Status503ServiceUnavailable);

                options.MapToStatusCode<Exception>(statusCode: StatusCodes.Status500InternalServerError);
            });
        }
    }
}