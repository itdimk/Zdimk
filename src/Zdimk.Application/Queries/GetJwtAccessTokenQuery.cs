using MediatR;

namespace Zdimk.Application.Queries
{
    public class GetJwtAccessTokenQuery : IRequest<string>
    {
        public string JwtRefreshToken { get; set; }
    }
}