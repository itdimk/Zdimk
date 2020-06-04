using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Frontend.Extensions;

namespace Zdimk.Application.Frontend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMediator _mediator;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IMediator mediator, ILocalStorageService localStorage)
        {
            _mediator = mediator;
            _localStorage = localStorage;
        }

        public async Task<bool> SignInAsync(string login, string password)
        {
            var getTokenPairQuery = new GetTokenPairQuery
            {
                Login = login,
                Password = password
            };

            JwtTokenPair tokenPair = await _mediator.Send(getTokenPairQuery);

            if (tokenPair != null)
            {
                await _localStorage.SetAccessTokenAsync(tokenPair.AccessToken);
                await _localStorage.SetRefreshTokenAsync(tokenPair.RefreshToken);

                var activateRefreshTokenCommand = new ActivateRefreshTokenCommand
                {
                    RefreshToken = tokenPair.RefreshToken,
                    Thumbprint = "12345" // TODO: make HWID
                };

                await _mediator.Send(activateRefreshTokenCommand);
                return true;
            }

            return false;
        }

        public async Task<ClaimsPrincipal> GetAuthenticatedUserAsync()
        {
            string encodedToken = await _localStorage.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(encodedToken))
                return null;

            var accessToken = new JwtSecurityToken(encodedToken);

            if (!VerifyTokenLifetime(accessToken))
            {
                string accessTokenString = await GetNewAccessTokenAsync();

                if (accessTokenString != null)
                {
                    await _localStorage.SetAccessTokenAsync(accessTokenString);
                    accessToken = new JwtSecurityToken(accessTokenString);
                }
                else
                    return null;
            }


            string userId = accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string userName = accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (userId != null && userName != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName),
                }, "Bearer");

                return new ClaimsPrincipal(identity);
            }
            else
            {
                throw new Exception("Invalid access token received");
            }
        }

        public async Task<bool> IsAuthorizedAsync()
        {
            return await GetAuthenticatedUserAsync() != null;
        }

        private bool VerifyTokenLifetime(JwtSecurityToken token)
        {
            return token.ValidTo > DateTime.UtcNow;
        }

        private async Task<string> GetNewAccessTokenAsync()
        {
            var refreshTokenString = await _localStorage.GetRefreshTokenAsync();
            var refreshToken = new JwtSecurityToken(refreshTokenString);

            if (VerifyTokenLifetime(refreshToken))
            {
                var query = new GetAccessTokenQuery
                {
                    JwtRefreshToken = refreshTokenString,
                    Thumbprint = "12345"
                };

                return await _mediator.Send(query);
            }

            return null;
        }
    }
}