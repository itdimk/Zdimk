using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Commands;
using Zdimk.BlazorApp.Dtos.Queries;
using Zdimk.BlazorApp.Extensions;
using Zdimk.BlazorApp.Services.Configuration;

namespace Zdimk.BlazorApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly SecurityOptions _securityOptions;
        private readonly ILocalStorageService _localStore;

        public UserService(HttpClient httpClient, IOptions<SecurityOptions> securityOptions,
            ILocalStorageService localStore)
        {
            _httpClient = httpClient;
            _securityOptions = securityOptions.Value;
            _localStore = localStore;
        }

        public async Task<bool> SignIn(GetJwtTokenPairQuery query)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetJwtTokenPairUrl);
            var tokenPair = await _httpClient.PostAsJsonAsync<GetJwtTokenPairQuery, JwtTokenPair>(requestUrl, query);

            if (tokenPair != null)
            {
                await _localStore.SetItemAsync(_securityOptions.RefreshTokenName, tokenPair.RefreshToken);
                await _localStore.SetItemAsync(_securityOptions.AccessTokenName, tokenPair.AccessToken);

                
                var cmd = new ActivateJwtRefreshTokenCommand
                {
                    RefreshToken = tokenPair.RefreshToken,
                    Thumbprint = "12345"
                };
                
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", tokenPair.AccessToken);
                Uri activateTokenUrl = new Uri(_httpClient.BaseAddress, ApiConstants.ActivateRefreshToken);
                _httpClient.PostAsJsonAsync(activateTokenUrl, cmd, authHeader);
                
                return true;
            }

            return false;
        }

        public async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenString = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            var token = new JwtSecurityToken(tokenString);

            if (token.ValidTo > DateTime.UtcNow + TimeSpan.FromMinutes(1))
            {
                await RefreshAccessToken();
                tokenString = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
                token = new JwtSecurityToken(tokenString);
            }

            if (token.ValidTo > DateTime.UtcNow)
            {
                string userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string userName = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName),
                }, "Bearer");

                var user = new ClaimsPrincipal(identity);
                return await Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                var identity = new ClaimsIdentity();
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return new AuthenticationState(claimsPrincipal);
            }
        }

        private async Task RefreshAccessToken()
        {
            var refreshTokenString = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            var refreshToken = new JwtSecurityToken(refreshTokenString);

            if (refreshToken.ValidTo > DateTime.UtcNow)
            {
                Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetJwtAccessTokenUrl);
                var accessToken = await _httpClient.PostAsJsonAsync<GetJwtAccessTokenQuery, string>(requestUrl,
                    new GetJwtAccessTokenQuery()
                    {
                        JwtRefreshToken = refreshTokenString
                    });

                if (accessToken != null)
                {
                    await _localStore.SetItemAsync(_securityOptions.AccessTokenName, accessToken);
                }
            }
        }
    }
}