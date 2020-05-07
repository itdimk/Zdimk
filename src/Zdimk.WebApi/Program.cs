using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Zdimk.WebApi.Extensions;
using Environment = Zdimk.WebApi.Extensions.Environment;

namespace Zdimk.WebApi
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var environment = Environment.GetEnvironment();

            // Used to build key/value based configuration settings for use in an application.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath: Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: $"appsettings.{environment}.json", optional: true)
                .AddCommandLine(args: args)
                .Build();

            // Configuration object for creating <see cref="ILogger"/> instances.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration: configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information(messageTemplate: "Starting web host...");
                await CreateHostBuilder(args: args).Build().RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(exception: ex, messageTemplate: "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog();
                    webBuilder.ConfigureKestrelHost();
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}