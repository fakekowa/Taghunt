using TagHunt.Models;
using System.Threading.Tasks;

namespace TagHunt.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(string email, string password, string username);
        Task<User> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<User> GetCurrentUserAsync();
        Task<bool> IsUserLoggedInAsync();
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> UpdateUserProfileAsync(User user);
    }
} 