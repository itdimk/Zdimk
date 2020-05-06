using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Commands;

namespace Zdimk.BlazorApp.Shared
{
    public partial class AlbumForm
    {
        private bool _titleToggle;
        private string Title => _titleToggle ? "Edit album " : "Add album";

        [Inject] private IGalleryService Gallery { get; set; }

        private async void OnSubmit()
        {
             bool result = await Gallery.CreateAlbum(Model);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Model == null)
                {
                    Model = new CreateAlbumCommand();
                    _titleToggle = true;
                }
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }
    }
}