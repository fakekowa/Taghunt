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

        // Track if restoration is complete
        private Task? _restorationTask;
        private bool _restorationCompleted = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FirebaseAuthService
        /// </summary>
        /// <param name="apiKey">Firebase API key</param>
        /// <param name="authDomain">Firebase authentication domain</param>
        public FirebaseAuthService(string apiKey, string authDomain)
        {
            try
            {
                var config = new FirebaseAuthConfig
                {
                    ApiKey = apiKey,
                    AuthDomain = authDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
                };

#if DEBUG
                // For simulator debugging - add retry logic and extended timeouts
                System.Diagnostics.Debug.WriteLine("Firebase Auth configured for simulator with extended timeouts");
#endif

                _authClient = new FirebaseAuthClient(config);
                
                System.Diagnostics.Debug.WriteLine($"Firebase Auth initialized with API Key: {apiKey?.Substring(0, 10)}...");
                System.Diagnostics.Debug.WriteLine($"Firebase Auth Domain: {authDomain}");
                
                // Start restoration and track the task
                _restorationTask = RestoreAuthenticationStateAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase Auth initialization failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines if an exception is network-related and should be retried
        /// </summary>
        /// <param name="ex">The exception to check</param>
        /// <returns>True if the error is network-related</returns>
        private static bool IsNetworkRelatedError(Exception ex)
        {
            var message = ex.Message?.ToLowerInvariant() ?? "";
            var innerMessage = ex.InnerException?.Message?.ToLowerInvariant() ?? "";

            // Check for common network error patterns
            return message.Contains("network connection") ||
                   message.Contains("cannot parse response") ||
                   message.Contains("timeout") ||
                   message.Contains("connection lost") ||
                   innerMessage.Contains("network connection") ||
                   innerMessage.Contains("cannot parse response") ||
                   innerMessage.Contains("timeout") ||
                   innerMessage.Contains("connection lost") ||
                   ex.GetType().Name.Contains("HttpRequest") ||
                   ex.GetType().Name.Contains("Network") ||
                   ex.GetType().Name.Contains("Timeout");
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
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting registration for email: {email}");
                var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
                await userCredential.User.ChangeDisplayNameAsync(username);
                
                System.Diagnostics.Debug.WriteLine($"Registration successful for user: {userCredential.User.Uid}");
                
                // Store the new user credentials
                _currentUserCredential = userCredential;
                await StoreAuthenticationStateAsync(_currentUserCredential);

                return new Models.User
                {
                    Id = userCredential.User.Uid,
                    Email = userCredential.User.Info?.Email ?? email,
                    Username = username,
                    DisplayName = username
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Registration failed: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw new Exception($"Registration failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>The authenticated user</returns>
        public async Task<Models.User> LoginAsync(string email, string password)
        {
            // Retry logic for simulator networking issues
            var maxRetries = 3;
            var retryDelay = TimeSpan.FromSeconds(2);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Attempting login for email: {email} (attempt {attempt}/{maxRetries})");
                    
                    _currentUserCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
                    var user = _currentUserCredential.User;

                    System.Diagnostics.Debug.WriteLine($"Login successful for user: {user.Uid}");

                    // Store authentication state securely
                    await StoreAuthenticationStateAsync(_currentUserCredential);

                    return new Models.User
                    {
                        Id = user.Uid,
                        Email = user.Info?.Email ?? email,
                        Username = user.Info?.DisplayName ?? "",
                        DisplayName = user.Info?.DisplayName ?? ""
                    };
                }
                catch (Exception ex) when (attempt < maxRetries && IsNetworkRelatedError(ex))
                {
                    System.Diagnostics.Debug.WriteLine($"Login attempt {attempt} failed with network error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Retrying in {retryDelay.TotalSeconds} seconds...");
                    await Task.Delay(retryDelay);
                    continue;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Login failed: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    throw new Exception($"Login failed: {ex.Message}", ex);
                }
            }

            throw new Exception("Login failed after multiple attempts due to network issues");
        }

        /// <summary>
        /// Gets the currently authenticated user
        /// </summary>
        /// <returns>The current user or null if not authenticated</returns>
        public async Task<Models.User?> GetCurrentUserAsync()
        {
            // Ensure restoration is complete before checking user data
            if (_restorationTask != null && !_restorationCompleted)
            {
                await _restorationTask;
                _restorationCompleted = true;
            }

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
                System.Diagnostics.Debug.WriteLine($"GetCurrentUserAsync: Found active credential for user {result.Id}");
                return result;
            }

            // Check if we have a restored session from secure storage
            var hasRestoredSession = await SecureStorage.GetAsync("HasRestoredSession");
            System.Diagnostics.Debug.WriteLine($"GetCurrentUserAsync: HasRestoredSession = {hasRestoredSession}");
            
            if (hasRestoredSession == "true")
            {
                var userId = await SecureStorage.GetAsync(UserIdKey);
                var userEmail = await SecureStorage.GetAsync(UserEmailKey);
                var userDisplayName = await SecureStorage.GetAsync(UserDisplayNameKey);

                System.Diagnostics.Debug.WriteLine($"GetCurrentUserAsync: Restored user data - ID: {userId}, Email: {userEmail}");

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail))
                {
                    return new Models.User
                    {
                        Id = userId,
                        Email = userEmail,
                        Username = userDisplayName ?? "",
                        DisplayName = userDisplayName ?? ""
                    };
                }
            }

            System.Diagnostics.Debug.WriteLine("GetCurrentUserAsync: No user found");
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
            System.Diagnostics.Debug.WriteLine("LogoutAsync: Logging out user...");
            
            _currentUserCredential = null;
            
            // Reset restoration state
            _restorationCompleted = false;
            _restorationTask = null;
            
            // Clear stored authentication state
            await ClearAuthenticationStateAsync();
            
            System.Diagnostics.Debug.WriteLine("LogoutAsync: Logout completed successfully");
        }

        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        /// <returns>True if a user is logged in, false otherwise</returns>
        public async Task<bool> IsUserLoggedInAsync()
        {
            // Ensure restoration is complete before checking login status
            if (_restorationTask != null && !_restorationCompleted)
            {
                await _restorationTask;
                _restorationCompleted = true;
            }

            // First check active credential
            if (_currentUserCredential?.User != null)
            {
                return true;
            }

            // Check for restored session
            var hasRestoredSession = await SecureStorage.GetAsync("HasRestoredSession");
            var result = hasRestoredSession == "true";
            
            System.Diagnostics.Debug.WriteLine($"IsUserLoggedInAsync result: {result}, HasRestoredSession: {hasRestoredSession}");
            return result;
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

        /// <summary>
        /// Tests Firebase configuration and connectivity
        /// </summary>
        /// <returns>True if configuration is valid, throws exception with details if not</returns>
        public async Task<bool> TestConfigurationAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Testing Firebase configuration...");
                
                // Try a simple operation to test the configuration
                // We'll try to sign in with an invalid email/password to test if the API responds
                try
                {
                    await _authClient.SignInWithEmailAndPasswordAsync("test@invalid.com", "invalid");
                }
                catch (Exception ex)
                {
                    // We expect this to fail, but if it fails with configuration errors, that's what we want to catch
                    if (ex.Message.Contains("INVALID_API_KEY"))
                    {
                        throw new Exception("Invalid Firebase API Key. Please check your configuration.");
                    }
                    else if (ex.Message.Contains("PROJECT_NOT_FOUND"))
                    {
                        throw new Exception("Firebase project not found. Please verify the project ID.");
                    }
                    else if (ex.Message.Contains("PERMISSION_DENIED"))
                    {
                        throw new Exception("Firebase Authentication is not enabled for this project.");
                    }
                    else if (ex.Message.Contains("Undefined"))
                    {
                        throw new Exception("Firebase project configuration error. Please ensure Authentication is enabled in the Firebase Console.");
                    }
                    
                    // If it's just an invalid email/password error, that means the API is working
                    if (ex.Message.Contains("INVALID_EMAIL") || ex.Message.Contains("INVALID_PASSWORD") || ex.Message.Contains("USER_NOT_FOUND") || ex.Message.Contains("WEAK_PASSWORD"))
                    {
                        System.Diagnostics.Debug.WriteLine("Firebase configuration test passed (expected authentication failure)");
                        return true;
                    }
                    
                    // Re-throw if it's an unexpected error
                    throw new Exception($"Firebase configuration error: {ex.Message}");
                }
                
                System.Diagnostics.Debug.WriteLine("Firebase configuration test passed");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase configuration test failed: {ex.Message}");
                throw;
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
                    System.Diagnostics.Debug.WriteLine($"StoreAuthenticationStateAsync: Storing auth state for user {userCredential.User.Uid}");
                    
                    // Store auth tokens and user info securely
                    var idToken = await userCredential.User.GetIdTokenAsync();
                    
                    await SecureStorage.SetAsync(AuthTokenKey, idToken);
                    await SecureStorage.SetAsync(UserIdKey, userCredential.User.Uid);
                    await SecureStorage.SetAsync(UserEmailKey, userCredential.User.Info?.Email ?? "");
                    await SecureStorage.SetAsync(UserDisplayNameKey, userCredential.User.Info?.DisplayName ?? "");
                    await SecureStorage.SetAsync(LoginTimestampKey, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
                    
                    System.Diagnostics.Debug.WriteLine($"StoreAuthenticationStateAsync: Successfully stored auth state");
                    System.Diagnostics.Debug.WriteLine($"  UserID: {userCredential.User.Uid}");
                    System.Diagnostics.Debug.WriteLine($"  Email: {userCredential.User.Info?.Email}");
                    System.Diagnostics.Debug.WriteLine($"  DisplayName: {userCredential.User.Info?.DisplayName}");
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
                System.Diagnostics.Debug.WriteLine("RestoreAuthenticationStateAsync: Starting restoration...");
                
                var authToken = await SecureStorage.GetAsync(AuthTokenKey);
                var loginTimestamp = await SecureStorage.GetAsync(LoginTimestampKey);
                
                System.Diagnostics.Debug.WriteLine($"RestoreAuthenticationStateAsync: AuthToken exists: {!string.IsNullOrEmpty(authToken)}, LoginTimestamp: {loginTimestamp}");
                
                if (!string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(loginTimestamp))
                {
                    // Check if the stored login is not too old (e.g., 30 days)
                    if (long.TryParse(loginTimestamp, out var timestamp))
                    {
                        var loginTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                        var daysSinceLogin = (DateTimeOffset.UtcNow - loginTime).TotalDays;
                        
                        System.Diagnostics.Debug.WriteLine($"RestoreAuthenticationStateAsync: Days since login: {daysSinceLogin}");
                        
                        if (daysSinceLogin <= 30) // Keep login for 30 days
                        {
                            // Create a user info object from stored data
                            System.Diagnostics.Debug.WriteLine("RestoreAuthenticationStateAsync: Restoring user from stored data...");
                            await RestoreUserFromStoredDataAsync();
                        }
                        else
                        {
                            // Login is too old, clear it
                            System.Diagnostics.Debug.WriteLine("RestoreAuthenticationStateAsync: Login too old, clearing...");
                            await ClearAuthenticationStateAsync();
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("RestoreAuthenticationStateAsync: No stored authentication data found");
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
                
                System.Diagnostics.Debug.WriteLine($"RestoreUserFromStoredDataAsync: UserID: {userId}, Email: {userEmail}, DisplayName: {userDisplayName}");
                
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userEmail))
                {
                    // Set a flag that indicates we have a restored session
                    // We'll use this in GetCurrentUserAsync to return stored user data
                    await SecureStorage.SetAsync("HasRestoredSession", "true");
                    System.Diagnostics.Debug.WriteLine("RestoreUserFromStoredDataAsync: HasRestoredSession set to true");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("RestoreUserFromStoredDataAsync: Missing required user data, cannot restore session");
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