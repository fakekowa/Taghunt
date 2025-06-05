using TagHunt.Models;

namespace TagHunt.Services.Interfaces
{
    /// <summary>
    /// Interface for authentication services that handle user registration, login, and profile management
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided credentials
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <param name="username">User's username</param>
        /// <returns>The newly registered user</returns>
        Task<Models.User> RegisterUserAsync(string email, string password, string username);
        
        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>The authenticated user</returns>
        Task<Models.User> LoginAsync(string email, string password);
        
        /// <summary>
        /// Gets the currently authenticated user
        /// </summary>
        /// <returns>The current user or null if not authenticated</returns>
        Task<Models.User?> GetCurrentUserAsync();
        
        /// <summary>
        /// Updates the current user's profile information
        /// </summary>
        /// <param name="user">User with updated information</param>
        /// <returns>Task representing the async operation</returns>
        Task UpdateUserProfileAsync(Models.User user);
        
        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        Task LogoutAsync();
        
        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        /// <returns>True if a user is logged in, false otherwise</returns>
        Task<bool> IsUserLoggedInAsync();
        
        /// <summary>
        /// Sends a password reset email to the specified email address
        /// </summary>
        /// <param name="email">Email address to send reset link to</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> ResetPasswordAsync(string email);
        
        /// <summary>
        /// Tests Firebase configuration and connectivity
        /// </summary>
        /// <returns>True if configuration is valid, throws exception with details if not</returns>
        Task<bool> TestConfigurationAsync();
    }
} 