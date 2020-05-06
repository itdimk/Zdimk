namespace Zdimk.BlazorApp.Dtos.Queries
{
    public class GetJwtAccessTokenQuery 
    {
        public string JwtRefreshToken { get; set; }
        public string Thumbprint { get; set; }
    }
}