using MediatR;

namespace Zdimk.Abstractions.Queries
{
    public class GetAccessTokenQuery : IRequest<string>
    {
        public string JwtRefreshToken { get; set; }
        public string Thumbprint { get; set; }
    }
}