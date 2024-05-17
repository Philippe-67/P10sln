using Microsoft.Win32;
using MSUi.Models.Authentification;

namespace MSUi.Services
{
    public interface IAuthService
    {
        Task<AccountStatus> RegisterAsync(Register register);
        Task<AccountStatus> LoginAsync(Login loging);
        Task LogoutAsync();
    }
}
