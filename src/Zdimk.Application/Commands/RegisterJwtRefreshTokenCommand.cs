using MediatR;

namespace Zdimk.Application.Commands
{
    public class RegisterJwtRefreshTokenCommand : IRequest
    {
        public string RefreshToken { get; set; }
    }
}