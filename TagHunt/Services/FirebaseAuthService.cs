using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

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
        public Task<Models.User?> GetCurrentUserAsync()
        {
            if (_currentUserCredential?.User == null)
            {
                return Task.FromResult<Models.User?>(null);
            }

            var user = _currentUserCredential.User;
            var result = new Models.User
            {
                Id = user.Uid,
                Email = user.Info.Email,
                Username = user.Info.DisplayName,
                DisplayName = user.Info.DisplayName
            };
            return Task.FromResult<Models.User?>(result);
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
        public Task LogoutAsync()
        {
            _currentUserCredential = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        /// <returns>True if a user is logged in, false otherwise</returns>
        public Task<bool> IsUserLoggedInAsync()
        {
            return Task.FromResult(_currentUserCredential?.User != null);
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
    }
} 