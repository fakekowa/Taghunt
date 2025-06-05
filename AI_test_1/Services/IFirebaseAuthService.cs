using AI_test_1.Models;

namespace AI_test_1.Services;

public interface IFirebaseAuthService
{
    Task<User> SignInWithEmailAndPasswordAsync(string email, string password);
    Task<User> SignUpWithEmailAndPasswordAsync(string email, string password, string displayName);
    Task SignOutAsync();
    Task<User> GetCurrentUserAsync();
    Task<bool> IsSignedInAsync();
} 