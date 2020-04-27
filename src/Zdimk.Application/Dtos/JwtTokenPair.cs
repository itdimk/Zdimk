namespace Zdimk.Application.Dtos
{
    public class JwtTokenPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}