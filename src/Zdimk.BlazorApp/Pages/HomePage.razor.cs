using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zdimk.BlazorApp.Pages
{
    public partial class HomePage
    {
        private Guid UserId { get; set; }
        private string UserName { get; set; }
        
        string Title => _showAlbumView ? $"Albums of {UserName}" : "Add album";
        
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await UserService.GetAuthenticationStateAsync();
                
                string userIdString = authState?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string userNameString = authState?.User.FindFirstValue(ClaimTypes.Name);
                
                if (userIdString != null)
                {
                    UserId = Guid.Parse(userIdString);
                    UserName = userNameString;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        
        private void ToggleBlockBody()
        {
            _showAlbumView = !_showAlbumView;
            _showAlbumEdit = !_showAlbumEdit;
            StateHasChanged();
        }
    }
}