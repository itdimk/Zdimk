using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Frontend.Constants;
using Zdimk.Application.Frontend.QueryHandlers;
using Zdimk.Application.Frontend.Services;
using Zdimk.BlazorApp.Extensions;

namespace Zdimk.BlazorApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddBlazoredLocalStorage();
            
            services.AddHttpClient<IAuthService, AuthService>();
            services.AddQueryHandlers();
            services.AddCommandHandlers();
            services.AddSingleton(new HttpClient() { BaseAddress  = new Uri(ApiConstants.BaseApiUrl) });
            services.AddAuthService();
            services.AddMediatR(typeof(GetPicturesQuery).Assembly, typeof(GetPicturesQueryHandler).Assembly);
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