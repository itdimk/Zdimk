using MediatR;

namespace Zdimk.Abstractions.Commands
{
    public class ActivateRefreshTokenCommand : IRequest
    {
        public string RefreshToken { get; set; }
        public string Thumbprint { get; set; }
    }
}