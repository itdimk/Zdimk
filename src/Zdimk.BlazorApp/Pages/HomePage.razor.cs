using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.BlazorApp.Shared;

namespace Zdimk.BlazorApp.Pages
{
    public partial class HomePage
    {
        private bool ShowCreateAlbumModalToggle = false;
        private List<AlbumDto> Albums { get; } = new List<AlbumDto>();
        private DotNetObjectReference<HomePage> _objRef;
        private int _currentOffset = 0;

        private void ShowCreateAlbum()
        {
            ShowCreateAlbumModalToggle = true;
        }
        
        private void HideCreateAlbum()
        {
            ShowCreateAlbumModalToggle = false;
        }
        


        public HomePage()
        {
            _objRef = DotNetObjectReference.Create(this);
        }
        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
            if (firstRender)
            {
                await LoadMore();
            }

            await JsRuntime.InvokeAsync<string>("RegisterScrollTracking", new object[] { _objRef, nameof(LoadMore) });
            
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task<GetAlbumsQuery> CreateQuery()
        {
            var user = await Auth.GetAuthenticatedUserAsync();

            if (user != null)
            {
                return new GetAlbumsQuery
                {
                    UserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Count = 50,
                    Offset = _currentOffset,
                };
            }
            else
                return null;
        }

        [JSInvokable]
        public async Task LoadMore()
        {
            if (await Auth.IsAuthorizedAsync())
            {
                var query = await CreateQuery();

                var albums = await Mediator.Send(query);

                if (albums != null)
                {
                    Albums.AddRange(albums);
                    StateHasChanged();
                    _currentOffset += query.Count;
                }
            }
        }


        public void Dispose()
        {
            _objRef?.Dispose();
        }

        private void OnAlbumCreated(AlbumDto album)
        {
            Albums.Add(album);
            StateHasChanged();
        }
    }
}