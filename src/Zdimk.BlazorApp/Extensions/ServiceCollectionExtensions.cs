using Microsoft.Extensions.DependencyInjection;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Frontend.CommandHandlers;
using Zdimk.Application.Frontend.QueryHandlers;
using Zdimk.Application.Frontend.Services;

namespace Zdimk.BlazorApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // TODO: ?
        public static void AddCommandHandlers(this IServiceCollection services)
        {
          //  services.AddHttpClient<ActivateRefreshTokenCommandHandler>();
          //  services.AddHttpClient<CreateAlbumCommandHandler>();
          //  services.AddHttpClient<CreatePictureCommandHandler>();
        }
        
        public static void AddQueryHandlers(this IServiceCollection services)
        {
         //   services.AddHttpClient<GetAccessTokenQueryHandler>();
          //  services.AddHttpClient<GetAlbumsQueryHandler>();
          //  services.AddHttpClient<GetPicturesQueryHandler>();
          //  services.AddHttpClient<GetTokenPairQueryHandler>();
        }

        public static void AddAuthService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
        
    }
}