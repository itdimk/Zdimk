using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Zdimk.Application.Frontend.Constants;

namespace Zdimk.Application.Frontend.Extensions
{
    public static class LocalStorageExtensions
    {
        public static async Task<string> GetAccessTokenAsync(this ILocalStorageService localStorage)
            => await localStorage.GetItemAsync<string>(JwtConstants.AccessTokenName);

        public static async Task<string> GetRefreshTokenAsync(this ILocalStorageService localStorage)
            => await localStorage.GetItemAsync<string>(JwtConstants.RefreshTokenName);

        public static async Task SetAccessTokenAsync(this ILocalStorageService localStorage, string accessToken)
            => await localStorage.SetItemAsync(JwtConstants.AccessTokenName, accessToken);

        public static async Task SetRefreshTokenAsync(this ILocalStorageService localStorage, string refreshToken)
            => await localStorage.SetItemAsync(JwtConstants.RefreshTokenName, refreshToken);

        public static async Task<AuthenticationHeaderValue> GetAuthHeaderValueAsync(
            this ILocalStorageService localStorage)
        {
            string accessToken = await localStorage.GetAccessTokenAsync();
            return new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}