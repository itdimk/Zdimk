namespace Zdimk.Application.Frontend.Constants
{
    public static class ApiConstants
    {
        public const string BaseApiUrl = "https://localhost:5000";
        
        public const string GetAlbumsUrl = "Gallery/GetAlbums";
        public const string GetPicturesUrl = "Gallery/GetPictures";
        public const string GetTokenPairUrl = "Auth/GetJwtTokenPair";
        public const string GetAccessTokenUrl = "Auth/GetJwtAccessToken";
        public const string GetTagsUrl = "Gallery/GetTags";
        public const string CreateImage = "Gallery/CreatePicture";
        public const string CreateAlbum = "Gallery/CreateAlbum";
        public const string ActivateRefreshToken = "/Auth/RegisterJwtRefreshToken";
    }
}