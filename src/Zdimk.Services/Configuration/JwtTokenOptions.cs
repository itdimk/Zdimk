using System;

namespace Zdimk.Services.Configuration
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; }
        public string AccessTokenAudience { get; set; }
        public string RefreshTokenAudience { get; set; }
        public byte[] PrivateKey { get; set; }
        public string AccessTokenSigningAlgorithm { get; set; }
        public string RefreshTokenSigningAlgorithm { get; set; }
        public TimeSpan AccessTokenLifetime { get; set; }
        public TimeSpan RefreshTokenLifetime { get; set; }
    }
}