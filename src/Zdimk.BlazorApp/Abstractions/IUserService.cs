using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zdimk.BlazorApp.Dtos.Queries;

namespace Zdimk.BlazorApp.Abstractions
{
    public interface IUserService
    {
        Task<bool> SignIn(GetJwtTokenPairQuery query);
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }
}