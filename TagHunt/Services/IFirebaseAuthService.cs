using TagHunt.Models;

namespace TagHunt.Services;

/// <summary>
/// Interface for Firebase authentication services (legacy interface - consider using IAuthService instead)
/// </summary>
public interface IFirebaseAuthService
{
    /// <summary>
    /// Signs in a user with email and password
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <param name="password">User's password</param>
    /// <returns>The authenticated user</returns>
    Task<User> SignInWithEmailAndPasswordAsync(string email, string password);
    
    /// <summary>
    /// Signs up a new user with email, password, and display name
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <param name="password">User's password</param>
    /// <param name="displayName">User's display name</param>
    /// <returns>The newly created user</returns>
    Task<User> SignUpWithEmailAndPasswordAsync(string email, string password, string displayName);
    
    /// <summary>
    /// Signs out the current user
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task SignOutAsync();
    
    /// <summary>
    /// Gets the currently authenticated user
    /// </summary>
    /// <returns>The current user</returns>
    Task<User> GetCurrentUserAsync();
    
    /// <summary>
    /// Checks if a user is currently signed in
    /// </summary>
    /// <returns>True if a user is signed in, false otherwise</returns>
    Task<bool> IsSignedInAsync();
} 