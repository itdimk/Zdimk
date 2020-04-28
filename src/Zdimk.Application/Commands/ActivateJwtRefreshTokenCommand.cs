using MediatR;

namespace Zdimk.Application.Commands
{
    public class ActivateJwtRefreshTokenCommand : IRequest
    {
        public string RefreshToken { get; set; }
        public string Thumbprint { get; set; }
    }
}