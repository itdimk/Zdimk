using MediatR;
using Zdimk.Application.Dtos;

namespace Zdimk.Application.Queries
{
    public class GetJwtTokenPairQuery : IRequest<JwtTokenPair>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}