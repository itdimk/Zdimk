using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Zdimk.BlazorApp.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static void ConfigureKestrelHost(this IWebHostBuilder builder)
        {
            builder.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Listen(IPAddress.Any, 5000,
                    listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
            });
        }
    }
}