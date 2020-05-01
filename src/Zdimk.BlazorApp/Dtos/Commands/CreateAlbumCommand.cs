namespace Zdimk.BlazorApp.Dtos.Commands
{
    public class CreateAlbumCommand 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
    }
}