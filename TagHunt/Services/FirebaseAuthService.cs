using System;
using System.Threading.Tasks;
using Firebase.Auth;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;
        private Firebase.Auth.User? _currentUser;

        public FirebaseAuthService(string apiKey)
        {
            _authClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = apiKey
            });
        }

        public async Task<Models.User> RegisterUserAsync(string email, string password, string username)
        {
            var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
            var firebaseUser = userCredential.User;
            await firebaseUser.UpdateUserProfileAsync(new UserProfile { DisplayName = username });

            return new Models.User
            {
                Id = firebaseUser.Uid,
                Email = firebaseUser.Info.Email,
                Username = username
            };
        }

        public async Task<Models.User> LoginAsync(string email, string password)
        {
            var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            _currentUser = userCredential.User;

            return new Models.User
            {
                Id = _currentUser.Uid,
                Email = _currentUser.Info.Email,
                Username = _currentUser.Info.DisplayName
            };
        }

        public async Task<Models.User> GetCurrentUserAsync()
        {
            if (_currentUser == null) throw new InvalidOperationException("No user is currently logged in");

            return new Models.User
            {
                Id = _currentUser.Uid,
                Email = _currentUser.Info.Email,
                Username = _currentUser.Info.DisplayName
            };
        }

        public async Task<bool> UpdateUserProfileAsync(Models.User user)
        {
            try
            {
                if (_currentUser == null) throw new InvalidOperationException("No user is currently logged in");

                await _currentUser.UpdateUserProfileAsync(new UserProfile { DisplayName = user.Username });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                _currentUser = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> IsUserLoggedInAsync()
        {
            return Task.FromResult(_currentUser != null);
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