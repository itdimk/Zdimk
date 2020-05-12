using System;
using System.Collections.Generic;
using System.Text;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Zdimk.Abstractions.Commands;
using Zdimk.Application.CommandHandlers;
using Zdimk.Application.Implementations.Configuration;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;
using Zdimk.WebApi.Extensions;

namespace Zdimk.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
            services.Configure<PictureServiceOptions>(op => op.PictureFolderName = "images");
            
            services.AddControllers();
            services.AddSwaggerGenerator();

            services.AddHttpContextAccessor();
            services.AddPictureService();
            
            services.AddMediatR(typeof(CreateUserCommand), typeof(CreatePictureCommandHandler));
            
            services.AddAuthorizationBundle<User, Guid>();
            services.AddAuthenticationBundle(opt =>
            {
                opt.Issuer = Configuration["Jwt:Issuer"];
                opt.AccessTokenAudience = Configuration["Jwt:AccessTokenAudience"];
                opt.RefreshTokenAudience = Configuration["Jwt:RefreshTokenAudience"];
                opt.PrivateKey = Encoding.UTF8.GetBytes(Configuration["Jwt:PrivateKey"]);
                opt.AccessTokenSigningAlgorithm = SecurityAlgorithms.HmacSha256Signature;
                opt.RefreshTokenSigningAlgorithm = SecurityAlgorithms.HmacSha512Signature;
                opt.AccessTokenLifetime = TimeSpan.FromMinutes(2);
                opt.RefreshTokenLifetime = TimeSpan.FromMinutes(50);
            });
            
            services.AddDbContext<MainDbContext>(opts => opts
                .UseNpgsql(Configuration.GetConnectionString("Default"))
                .UseLazyLoadingProxies());
            
            services.AddHellangProblemDetails();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", name: "ZdimkApiV1");
                });
            }

            app.UseRouting();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseProblemDetails();


            app.UseEndpoints(endpoints =>  endpoints.MapControllers());
        }
    }
}