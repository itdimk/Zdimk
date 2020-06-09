using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;

namespace Zdimk.BlazorApp.Pages
{
    public partial class PicturesPage
    {
        [Parameter] public string AlbumId { get; set; }

        private Guid AlbumGuid
        {
            get
            {
                if (Guid.TryParse(AlbumId, out Guid albumGuid))
                    return albumGuid;
                return default;
            }
        }

        private List<PictureDto> Pictures { get; set; } = new List<PictureDto>();

        private DotNetObjectReference<PicturesPage> _objRef;
        private int _currentOffset = 0;

        public PicturesPage()
        {
            _objRef = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && AlbumGuid != default)
            {
                await LoadMore();
                await JsRuntime.InvokeAsync<string>("RegisterScrollTracking", new object[] {_objRef, nameof(LoadMore)});
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private GetPicturesQuery CreateQuery()
        {
            return new GetPicturesQuery
            {
                AlbumId = AlbumGuid,
                Offset = _currentOffset,
                Count = 30,
            };
        }

        [JSInvokable]
        public async Task LoadMore()
        {
            if (await Auth.IsAuthorizedAsync())
            {
                var query = CreateQuery();

                var pictures = await Mediator.Send(query);

                if (pictures != null)
                {
                    Pictures.AddRange(pictures);
                    StateHasChanged();
                    _currentOffset += query.Count;
                }
            }
        }

        private void DeletePicture(PictureDto picture)
        {
            
        }

        private async void UploadPictureClicked()
        {
            NavManager.NavigateTo($"/albums/{AlbumId}/upload");
        }
    }
}