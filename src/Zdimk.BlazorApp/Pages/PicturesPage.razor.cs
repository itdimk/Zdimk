using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorInputFile;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Commands;
using Zdimk.BlazorApp.Dtos.Queries;
using Zdimk.BlazorApp.Services;

namespace Zdimk.BlazorApp.Pages
{
    public partial class PicturesPage
    {
        private List<PictureDto> Pictures { get; set; }
            = new List<PictureDto>();

        private async void OnFileChanged(IFileListEntry[] files)
        {
            if (files != null)
            {
                var commands = files.Select(f => new CreatePictureCommand()
                {
                    PictureFile = f,
                    AlbumId = AlbumId,
                    Name = "No name",
                    Description = "No description"
                });

                foreach (var command in commands)
                {
                    var picture = await Gallery.UploadPicture(command);

                    if (picture != null)
                    {
                        Pictures.Add(picture);
                        StateHasChanged();
                    }
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var query = new GetPicturesQuery
                {
                    AlbumId = AlbumId, Count = 20, Offset = 1
                };
                var pictures = await Gallery.GetPicturesAsync(query);
                if (pictures != null)
                {
                    Pictures.AddRange(pictures);
                    StateHasChanged();
                }
            }
        }

        private async void ShowOpenFileDialog()
        {
            await JsInterop.InvokeAsync<string>("invokeClickFor", new object[] {"fileUpload"});
        }
    }
}