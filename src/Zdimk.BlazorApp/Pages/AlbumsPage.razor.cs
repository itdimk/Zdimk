using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;

namespace Zdimk.BlazorApp.Pages
{
    public partial class AlbumsPage
    {
        private IEnumerable<AlbumDto> Albums { get; set; } = new List<AlbumDto>();

        [Parameter] public string UserName { get; set; }
        [Inject] private  IUserService UserService { get; set; }
        [Inject] private IGalleryService GalleryService { get; set; }
        
        
        protected  override async Task OnAfterRenderAsync(bool firstRender)
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

                
                Albums = await GalleryService.GetAlbumsAsync(query);
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}