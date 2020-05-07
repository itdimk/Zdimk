using System.Security.Claims;
using System.Threading.Tasks;

namespace Zdimk.Application.Frontend.Services
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string login, string password);

        Task<ClaimsIdentity> GetAuthenticatedUserAsync();
    }
}