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
            var authState = await Auth.GetAuthenticatedUserAsync();

            if (authState != null)
            {
                string newAuthorizedUserName = authState.FindFirstValue(ClaimTypes.Name);
                if (!string.Equals(AuthorizedUserName, newAuthorizedUserName))
                {
                    AuthorizedUserName = newAuthorizedUserName;
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