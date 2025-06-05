using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using TagHunt.Models;
using TagHunt.Services.Interfaces;
using Microsoft.Maui.Storage;

namespace TagHunt.Services
{
    /// <summary>
    /// Firebase implementation of the authentication service
    /// </summary>
    public class FirebaseAuthService : IAuthService
    {
        #region Fields

        private readonly FirebaseAuthClient _authClient;
        private Firebase.Auth.UserCredential? _currentUserCredential;
        
        // Secure storage keys
        private const string AuthTokenKey = "auth_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string UserIdKey = "user_id";
        private const string UserEmailKey = "user_email";
        private const string UserDisplayNameKey = "user_display_name";
        private const string LoginTimestampKey = "login_timestamp";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FirebaseAuthService
        /// </summary>
        /// <param name="apiKey">Firebase API key</param>
        /// <param name="authDomain">Firebase authentication domain</param>
        public FirebaseAuthService(string apiKey, string authDomain)
        {
            _authClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = apiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });
            
            // Try to restore previous authentication state
            _ = Task.Run(async () => await RestoreAuthenticationStateAsync());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a new user with email, password, and username
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <param name="username">User's username</param>
        /// <returns>The created user</returns>
        public async Task<Models.User> RegisterUserAsync(string email, string password, string username)
        {
            var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
            await userCredential.User.ChangeDisplayNameAsync(username);
            
            // Store the new user credentials
            _currentUserCredential = userCredential;
            await StoreAuthenticationStateAsync(_currentUserCredential);

            return new Models.User
            {
                Id = userCredential.User.Uid,
                Email = userCredential.User.Info.Email,
                Username = username,
                DisplayName = username
            };
        }

        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>The authenticated user</returns>
        public async Task<Models.User> LoginAsync(string email, string password)
        {
            _currentUserCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            var user = _currentUserCredential.User;

            // Store authentication state securely
            await StoreAuthenticationStateAsync(_currentUserCredential);

            return new Models.User
            {
                Id = user.Uid,
                Email = user.Info.Email,
                Username = user.Info.DisplayName,
                DisplayName = user.Info.DisplayName
            };
        }

        /// <summary>
        /// Gets the currently authenticated user
        /// </summary>
        /// <returns>The current user or null if not authenticated</returns>
        public async Task<Models.User?> GetCurrentUserAsync()
        {
            // First check if we have an active credential
            if (_currentUserCredential?.User != null)
            {
                var user = _currentUserCredential.User;
                var result = new Models.User
                {
                    Id = user.Uid,
                    Email = user.Info.Email,
                    Username = user.Info.DisplayName,
                    DisplayName = user.Info.DisplayName
                };
                return result;
            }

            // Check if we have a restored session from secure storage
            var hasRestoredSession = await SecureStorage.GetAsync("HasRestoredSession");
            if (hasRestoredSession == "true")
            {
                var userId = await SecureStorage.GetAsync(UserIdKey);
                var userEmail = await SecureStorage.GetAsync(UserEmailKey);
                var userDisplayName = await SecureStorage.GetAsync(UserDisplayNameKey);

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail))
                {
                    return new Models.User
                    {
                        Id = userId,
                        Email = userEmail,
                        Username = userDisplayName,
                        DisplayName = userDisplayName
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the current user's profile information
        /// </summary>
        /// <param name="user">User with updated information</param>
        /// <returns>Task representing the async operation</returns>
        public async Task UpdateUserProfileAsync(Models.User user)
        {
            if (_currentUserCredential?.User == null)
            {
                throw new InvalidOperationException("No user is currently logged in");
            }
            await _currentUserCredential.User.ChangeDisplayNameAsync(user.Username);
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        public async Task LogoutAsync()
        {
            _currentUserCredential = null;
            
            // Clear stored authentication state
            await ClearAuthenticationStateAsync();
        }

        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        /// <returns>True if a user is logged in, false otherwise</returns>
        public async Task<bool> IsUserLoggedInAsync()
        {
            // First check active credential
            if (_currentUserCredential?.User != null)
            {
                return true;
            }

            // Check for restored session
            var hasRestoredSession = await SecureStorage.GetAsync("HasRestoredSession");
            return hasRestoredSession == "true";
        }

        /// <summary>
        /// Sends a password reset email to the specified email address
        /// </summary>
        /// <param name="email">Email address to send reset link to</param>
        /// <returns>True if successful, throws exception if failed</returns>
        public async Task<bool> ResetPasswordAsync(string email)
        {
            try
            {
                await _authClient.ResetEmailPasswordAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password reset failed: {ex.Message}");
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Stores authentication state securely for persistent login
        /// </summary>
        /// <param name="userCredential">The user credential to store</param>
        private async Task StoreAuthenticationStateAsync(Firebase.Auth.UserCredential userCredential)
        {
            try
            {
                if (userCredential?.User != null)
                {
                    // Store auth tokens and user info securely
                    var idToken = await userCredential.User.GetIdTokenAsync();
                    
                    await SecureStorage.SetAsync(AuthTokenKey, idToken);
                    await SecureStorage.SetAsync(UserIdKey, userCredential.User.Uid);
                    await SecureStorage.SetAsync(UserEmailKey, userCredential.User.Info.Email ?? "");
                    await SecureStorage.SetAsync(UserDisplayNameKey, userCredential.User.Info.DisplayName ?? "");
                    await SecureStorage.SetAsync(LoginTimestampKey, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - app should still work without persistent login
                System.Diagnostics.Debug.WriteLine($"Failed to store authentication state: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores authentication state from secure storage
        /// </summary>
        private async Task RestoreAuthenticationStateAsync()
        {
            try
            {
                var authToken = await SecureStorage.GetAsync(AuthTokenKey);
                var loginTimestamp = await SecureStorage.GetAsync(LoginTimestampKey);
                
                if (!string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(loginTimestamp))
                {
                    // Check if the stored login is not too old (e.g., 30 days)
                    if (long.TryParse(loginTimestamp, out var timestamp))
                    {
                        var loginTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                        var daysSinceLogin = (DateTimeOffset.UtcNow - loginTime).TotalDays;
                        
                        if (daysSinceLogin <= 30) // Keep login for 30 days
                        {
                            // Create a user info object from stored data
                            await RestoreUserFromStoredDataAsync();
                        }
                        else
                        {
                            // Login is too old, clear it
                            await ClearAuthenticationStateAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - app should still work
                System.Diagnostics.Debug.WriteLine($"Failed to restore authentication state: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores user data from stored information for persistent session
        /// </summary>
        private async Task RestoreUserFromStoredDataAsync()
        {
            try
            {
                var userId = await SecureStorage.GetAsync(UserIdKey);
                var userEmail = await SecureStorage.GetAsync(UserEmailKey);
                var userDisplayName = await SecureStorage.GetAsync(UserDisplayNameKey);
                
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail))
                {
                    // Set a flag that indicates we have a restored session
                    // We'll use this in GetCurrentUserAsync to return stored user data
                    await SecureStorage.SetAsync("HasRestoredSession", "true");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to restore user from stored data: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears all stored authentication state
        /// </summary>
        private async Task ClearAuthenticationStateAsync()
        {
            try
            {
                SecureStorage.Remove(AuthTokenKey);
                SecureStorage.Remove(RefreshTokenKey);
                SecureStorage.Remove(UserIdKey);
                SecureStorage.Remove(UserEmailKey);
                SecureStorage.Remove(UserDisplayNameKey);
                SecureStorage.Remove(LoginTimestampKey);
                SecureStorage.Remove("HasRestoredSession");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to clear authentication state: {ex.Message}");
            }
            
            await Task.CompletedTask;
        }

        #endregion
    }
} 