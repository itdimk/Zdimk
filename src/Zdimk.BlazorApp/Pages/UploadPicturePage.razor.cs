using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Commands;
using Zdimk.BlazorApp.Dtos.Queries;
using Zdimk.BlazorApp.Services;

namespace Zdimk.BlazorApp.Pages
{
    public partial class UploadPicturePage
    {
        private IEnumerable<AlbumDto> AvailableAlbums { get; set; } = new List<AlbumDto>();
        private CreatePictureCommand Command { get; set; } = new CreatePictureCommand();
        
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IGalleryService GalleryService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                string currentUserId = (await UserService.GetAuthenticationStateAsync())
                    .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var query = new GetAlbumsQuery
                {
                    UserName = currentUserId,
                    Offset = 0,
                    Count = 20,
                };

                AvailableAlbums = await GalleryService.GetAlbumsAsync(query);
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async void OnFileChanged(IFileListEntry[] files)
        {
            IFileListEntry file = files.FirstOrDefault();

            if (file != null)
                Command.PictureFile = file;


        }

        private async void OnSubmit()
        {
            await GalleryService.UploadPicture(Command);
        }
    }
}