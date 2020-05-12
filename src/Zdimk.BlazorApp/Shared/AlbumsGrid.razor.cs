using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;

namespace Zdimk.BlazorApp.Shared
{
    public partial class AlbumsGrid : IDisposable
    {
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Parameter] public  Guid UserId { get; set; }

        private DotNetObjectReference<AlbumsGrid> _objRef;
        
        private IEnumerable<AlbumDto> Albums { get; set; }


        public AlbumsGrid()
        {
            _objRef = DotNetObjectReference.Create(this);
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
            if (firstRender)
            {
                if (await Auth.IsAuthorizedAsync())
                {
                    var query = CreateQuery();

                    Albums = await Mediator.Send(query);
                    StateHasChanged();
                }
            }

            await JsRuntime.InvokeAsync<string>("RegisterScrollTracking", new object[] { _objRef, nameof(LoadMore) });
            
            await base.OnAfterRenderAsync(firstRender);
        }

        private GetAlbumsQuery CreateQuery() => new GetAlbumsQuery
        {
            UserId = UserId,
            Count = 20,
            Offset = 1
        };

        [JSInvokable]
        public async Task LoadMore()
        {
            int a = 0;
            a += 3;
            
        }


        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}