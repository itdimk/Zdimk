using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;
using Zdimk.BlazorApp.Extensions;

namespace Zdimk.BlazorApp.Pages
{
    public partial class AuthPage
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private  NavigationManager NavigationManager { get; set; }
        
        private GetJwtTokenPairQuery Model { get; } = new GetJwtTokenPairQuery();

        private bool InvalidData { get; set; }
        
        private async void OnSubmit()
        {
            if (await UserService.SignIn(Model))
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                InvalidData = true; 
                StateHasChanged();
            }
        }

        private void ResetInvalidData()
        {
            InvalidData = false;
        }
    }
}