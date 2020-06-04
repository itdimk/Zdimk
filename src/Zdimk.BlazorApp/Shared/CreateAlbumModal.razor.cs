using Microsoft.AspNetCore.Components;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.BlazorApp.Shared
{
    public partial class CreateAlbumModal
    {
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public EventCallback Hide { get; set; }
        [Parameter] public EventCallback<AlbumDto> AlbumCreated { get; set; }
        
        private string IsVisibleCss => IsVisible ? "" : "hidden";

        private CreateAlbumCommand Command { get; } = new CreateAlbumCommand();

        private async void OnSubmit()
        {
            bool isAuthorized = await Auth.IsAuthorizedAsync();

            if (isAuthorized)
            {
                AlbumDto album = await Mediator.Send(Command);

                if (album != null)
                {
                    await Hide.InvokeAsync(new object());
                    await AlbumCreated.InvokeAsync(album);
                }
            }
        }
    }
}