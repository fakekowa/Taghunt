using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;
        private Firebase.Auth.UserCredential? _currentUserCredential;

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

        public async Task UpdateUserProfileAsync(Models.User user)
        {
            if (_currentUserCredential?.User == null)
            {
                throw new InvalidOperationException("No user is currently logged in");
            }
            await _currentUserCredential.User.ChangeDisplayNameAsync(user.Username);
        }

        public Task LogoutAsync()
        {
            _currentUserCredential = null;
            return Task.CompletedTask;
        }

        public Task<bool> IsUserLoggedInAsync()
        {
            return Task.FromResult(_currentUserCredential?.User != null);
        }

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
    }
} 