namespace Zdimk.BlazorApp.Dtos.Queries
{
    public class GetAlbumsQuery 
    {
        public string UserName { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}