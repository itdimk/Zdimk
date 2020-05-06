using System.Collections.Generic;
using System.Threading.Tasks;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;

namespace Zdimk.BlazorApp.Shared
{
    public partial class AlbumGrid
    {
        private IEnumerable<AlbumDto> Model { get; set; } = new List<AlbumDto>();
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                if (UserId != null)
                {
                    var query = new GetAlbumsQuery
                    {
                        UserId = UserId.Value,
                        Offset = 0,
                        Count = 20,
                    };

                    Model = await GalleryService.GetAlbumsAsync(query);
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}