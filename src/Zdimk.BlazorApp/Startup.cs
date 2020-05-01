using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Services;
using Zdimk.BlazorApp.Services.Configuration;

namespace Zdimk.BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

            services.Configure<SecurityOptions>(opts =>
            {
                opts.AccessTokenName = "jwt-access";
                opts.RefreshTokenName = "jwt-refresh";
            });
            
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddBlazoredLocalStorage();
            

            services.AddHttpClient<IGalleryService, GalleryService>(h =>
                h.BaseAddress = new Uri(Configuration["BaseUrl"]));

            services.AddHttpClient<IUserService, UserService>(h =>
                h.BaseAddress = new Uri(Configuration["BaseUrl"]));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}