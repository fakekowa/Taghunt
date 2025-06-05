using TagHunt.Models;

namespace TagHunt.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Models.User> RegisterUserAsync(string email, string password, string username);
        Task<Models.User> LoginAsync(string email, string password);
        Task<Models.User?> GetCurrentUserAsync();
        Task UpdateUserProfileAsync(Models.User user);
        Task LogoutAsync();
    }
} 