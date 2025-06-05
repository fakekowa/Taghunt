using AI_test_1.Models;
using System.Threading.Tasks;

namespace AI_test_1.Services
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