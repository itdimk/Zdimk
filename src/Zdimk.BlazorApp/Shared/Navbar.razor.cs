using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Zdimk.BlazorApp.Abstractions;

namespace Zdimk.BlazorApp.Shared
{
    public partial class Navbar
    {
        bool IsCollapsed = true;

        [Inject] private IUserService UserService { get; set; }
        string AuthorizedUserName { get; set; }

        private void ToggleIsCollapsed()
        {
            IsCollapsed = !IsCollapsed;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var authState = await UserService.GetAuthenticationStateAsync();
            AuthorizedUserName = authState.User.FindFirstValue(ClaimTypes.Name);
            StateHasChanged();
        }
    }
}