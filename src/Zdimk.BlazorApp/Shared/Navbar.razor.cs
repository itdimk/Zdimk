using System.Security.Claims;
using System.Threading.Tasks;

namespace Zdimk.BlazorApp.Shared
{
    public partial class Navbar
    {
        private string AuthorizedUserName { get; set; }
        private bool IsCollapsed { get; set; } = true;
        private string IsCollapsedCss => IsCollapsed ? "is-collapsed" : "";
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await Auth.GetAuthenticatedUserAsync();

                if (authState != null)
                {
                    AuthorizedUserName = authState.FindFirstValue(ClaimTypes.Name);
                    StateHasChanged();

                }
            }
             await base.OnAfterRenderAsync(firstRender);
        }

        private void ToggleIsCollapsed()
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}