using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;

namespace Zdimk.BlazorApp.Shared
{
    public partial class PictureGrid
    {
        [Parameter] public ICollection<PictureDto> Pictures { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Pictures == null && firstRender)
            {
                var query = new GetPicturesQuery
                {
                    AlbumId = AlbumId,
                    Count = 20,
                    Offset = 1
                };

                Pictures = await Gallery.GetPicturesAsync(query);
                StateHasChanged();
            }
        }
    }
}