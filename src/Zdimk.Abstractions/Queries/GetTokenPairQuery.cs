using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Queries
{
    public class GetTokenPairQuery : IRequest<JwtTokenPair>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}