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
        [Parameter] public ICollection<PictureDto> Model { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Model == null && firstRender)
            {
                var query = new GetPicturesQuery
                {
                    AlbumId = AlbumId,
                    Count = 20,
                    Offset = 1
                };

                Model = await Gallery.GetPicturesAsync(query);
            }
        }
    }
}