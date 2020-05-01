namespace Zdimk.BlazorApp.Dtos.Commands
{
    public class ActivateJwtRefreshTokenCommand 
    {
        public string RefreshToken { get; set; }
        public string Thumbprint { get; set; }
    }
}