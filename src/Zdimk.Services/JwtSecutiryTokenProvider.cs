using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zdimk.Services.Configuration;
using Zdimk.Services.Constants;

namespace Zdimk.Services
{
    public class JwtSecutiryTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : IdentityUser
    {
        private readonly JwtSecurityTokenOptions _options;

        public JwtSecutiryTokenProvider(JwtSecurityTokenOptions options)
        {
            _options = options;
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

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            throw new NotImplementedException();
        }

        private JwtSecurityToken GenerateAccessToken(SymmetricSecurityKey key, UserManager<TUser> manager, TUser user)
        {
            var signingCredentials = new SigningCredentials(key, _options.AccessTokenSigningAlgorithm);

            return new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                expires: DateTime.Now + _options.AccessTokenLifetime);
        }
        
        private JwtSecurityToken GenerateRefreshToken(SymmetricSecurityKey key, UserManager<TUser> manager, TUser user)
        {
            var signingCredentials = new SigningCredentials(key, _options.RefreshTokenSigningAlgorithm);

            return new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                expires: DateTime.Now + _options.RefreshTokenLifetime);
        }
    }
}