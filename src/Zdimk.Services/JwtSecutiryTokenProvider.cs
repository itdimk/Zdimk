using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zdimk.Application.Constants;
using Zdimk.Services.Configuration;

namespace Zdimk.Services
{
    public class JwtSecutiryTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : IdentityUser
    {
        private readonly JwtTokenOptions _options;

        public JwtSecutiryTokenProvider(IOptions<JwtTokenOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var privateKey = new SymmetricSecurityKey(_options.PrivateKey);

            JwtSecurityToken token;
            if (purpose == JwtSecurityTokenPurposes.Access)
                token = await Task.Run(() => GenerateAccessToken(privateKey, manager, user));
            else if (purpose == JwtSecurityTokenPurposes.Refresh)
                token = await Task.Run(() => GenerateRefreshToken(privateKey, manager, user));
            else
                throw new ArgumentException(nameof(purpose));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
 
                jwtHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidIssuer = _options.Issuer,
                    ValidAudience = GetValidAudience(purpose),
                    IssuerSigningKey = new SymmetricSecurityKey(_options.PrivateKey)
                }, out SecurityToken validatedToken);

                return validatedToken != null;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            throw new NotImplementedException();
        }

        private JwtSecurityToken GenerateAccessToken(SymmetricSecurityKey key, UserManager<TUser> manager, TUser user)
        {
            var signingCredentials = new SigningCredentials(key, _options.AccessTokenSigningAlgorithm);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            
            return new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.AccessTokenAudience,
                claims: claims,
                expires: DateTime.Now + _options.AccessTokenLifetime,
                signingCredentials:signingCredentials);
        }
        
        private JwtSecurityToken GenerateRefreshToken(SymmetricSecurityKey key, UserManager<TUser> manager, TUser user)
        {
            var signingCredentials = new SigningCredentials(key, _options.RefreshTokenSigningAlgorithm);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            
            return new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.RefreshTokenAudience,
                claims: claims,
                expires: DateTime.Now + _options.RefreshTokenLifetime,
                signingCredentials:signingCredentials);
        }

        private string GetValidAudience(string purpose)
        {
            if (purpose == JwtSecurityTokenPurposes.Access)
                return _options.AccessTokenAudience;
            if (purpose == JwtSecurityTokenPurposes.Refresh)
                return _options.RefreshTokenAudience;
            return null;
        }
    }
}