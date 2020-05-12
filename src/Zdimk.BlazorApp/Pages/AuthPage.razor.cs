using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Zdimk.BlazorApp.Pages
{
    public partial class AuthPage
    {
        private string Login { get; set; }
        private string Password { get; set; }

        private bool IsInvalidData { get; set; }
        private bool IsWorking { get; set; }
        private string IsInvalidDataCss => IsInvalidData ? "is-invalid" : "";
        private string IsWorkingCss => IsWorking ? "is-working" : "";
        

        private void ToggleIsInvalidData()
        {
            IsInvalidData = !IsInvalidData;
            StateHasChanged();
        }
        
        private void ToggleIsWorking()
        {
            IsWorking = !IsWorking;
            StateHasChanged();
        }

        private async void OnSubmit()
        {
            IsWorking = true;
            StateHasChanged();
            
            bool result = await Auth.SignInAsync(Login, Password);


            IsWorking = false;
            if (result)
                NavManager.NavigateTo("/");
            else
            {
                IsInvalidData = true;
                StateHasChanged(); // TODO: inclute to getter
            }
        }

        private void OnInputFocus()
        {
            IsInvalidData = false;
            StateHasChanged(); // TODO: inclute to getter
        }
    }
}