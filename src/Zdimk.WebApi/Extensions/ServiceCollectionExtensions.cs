using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Zdimk.Domain.Entities;
using Zdimk.Services;

namespace Zdimk.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddPictureService(this IServiceCollection services)
        {
            services.AddSingleton<PictureService>();
        }

        public static void AddAuthorizationBundle<TUser>(this IServiceCollection services)
            where TUser: IdentityUser
        {
            services.AddAuthorization().AddIdentity<User, string>()
                .AddUserManager<User>()
                .AddRoleManager<User>()
                .AddSignInManager<User>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<JwtSecutiryTokenProvider<User>>("jwt")
                .AddDefaultTokenProviders();
        }
    }
}