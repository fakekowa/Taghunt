using TagHunt.Models;

namespace TagHunt.Services;

public interface IFirebaseAuthService
{
    Task<User> SignInWithEmailAndPasswordAsync(string email, string password);
    Task<User> SignUpWithEmailAndPasswordAsync(string email, string password, string displayName);
    Task SignOutAsync();
    Task<User> GetCurrentUserAsync();
    Task<bool> IsSignedInAsync();
} 