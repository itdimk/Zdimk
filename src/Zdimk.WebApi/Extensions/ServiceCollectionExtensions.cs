using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zdimk.Application.Interfaces;
using Zdimk.Services;

namespace Zdimk.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddPictureService(this IServiceCollection services)
        {
            services.AddSingleton<PictureService>();
        }
    }
}