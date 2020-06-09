using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Zdimk.BlazorApp.Pages
{
    public partial class AuthPage
    {
        private string Login { get; set; }
        private string Password { get; set; }

        private bool IsInvalidData { get; set; }
        private string IsInvalidDataCss => IsInvalidData ? "is-invalid" : "";
        

        private void ToggleIsInvalidData()
        {
            IsInvalidData = !IsInvalidData;
            StateHasChanged();
        }
        

        private async void OnSubmit()
        {
            StateHasChanged();
            
            bool result = await Auth.SignInAsync(Login, Password);

            if (result)
            {
                NavManager.NavigateTo("/");
                StateHasChanged();
            }
            else
            {
                IsInvalidData = true;
                StateHasChanged(); // TODO: include to getter
            }
        }

        private void OnInputFocus()
        {
            IsInvalidData = false;
            StateHasChanged(); // TODO: inclute to getter
        }
    }
}