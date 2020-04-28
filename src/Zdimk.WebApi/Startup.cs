using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Zdimk.Application.Commands;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;
using Zdimk.Services.Configuration;
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
            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo()));
            services.AddHttpContextAccessor();
            services.AddPictureService();
            
            services.AddMediatR(typeof(CreateUserCommand));
            
            services.AddAuthorizationBundle<User>();
            services.AddAuthenticationBundle(opt =>
            {
                opt.Issuer = Configuration["Jwt:Issuer"];
                opt.AccessTokenAudience = Configuration["Jwt:AccessTokenAudience"];
                opt.RefreshTokenAudience = Configuration["Jwt:RefreshTokenAudience"];
                opt.PrivateKey = Encoding.UTF8.GetBytes(Configuration["Jwt:PrivateKey"]);
                opt.AccessTokenSigningAlgorithm = SecurityAlgorithms.HmacSha256Signature;
                opt.RefreshTokenSigningAlgorithm = SecurityAlgorithms.HmacSha512Signature;
                opt.AccessTokenLifetime = TimeSpan.FromMinutes(10.0);
                opt.RefreshTokenLifetime = TimeSpan.FromDays(30.0);
            });
            
            services.AddDbContext<ZdimkDbContext>(opts => opts
                .UseNpgsql(Configuration.GetConnectionString("Default"))
                .UseLazyLoadingProxies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", name: "ZdimkApiV1"));
            }

            app.UseRouting();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>  endpoints.MapControllers());
        }
    }
}