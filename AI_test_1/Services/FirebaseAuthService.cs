using AI_test_1.Models;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Threading.Tasks;

namespace AI_test_1.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly FirebaseClient _database;
        private User _currentUser;

        public FirebaseAuthService(FirebaseAuthClient authClient, FirebaseClient database)
        {
            _authClient = authClient;
            _database = database;
        }

        public async Task<User> RegisterUserAsync(string email, string password, string username)
        {
            try
            {
                var authResult = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
                var user = new User
                {
                    Id = authResult.User.Uid,
                    Email = email,
                    Username = username,
                    DisplayName = username
                };

                await _database
                    .Child("users")
                    .Child(user.Id)
                    .PutAsync(user);

                _currentUser = user;
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                var authResult = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
                var user = await _database
                    .Child("users")
                    .Child(authResult.User.Uid)
                    .OnceSingleAsync<User>();

                user.LastLoginAt = DateTime.UtcNow;
                user.IsOnline = true;

                await _database
                    .Child("users")
                    .Child(user.Id)
                    .PatchAsync(new { LastLoginAt = user.LastLoginAt, IsOnline = true });

                _currentUser = user;
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                if (_currentUser != null)
                {
                    await _database
                        .Child("users")
                        .Child(_currentUser.Id)
                        .PatchAsync(new { IsOnline = false });
                }

                await _authClient.SignOutAsync();
                _currentUser = null;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Logout failed: {ex.Message}");
            }
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            var currentUser = _authClient.User;
            if (currentUser == null)
                return null;

            _currentUser = await _database
                .Child("users")
                .Child(currentUser.Uid)
                .OnceSingleAsync<User>();

            return _currentUser;
        }

        public Task<bool> IsUserLoggedInAsync()
        {
            return Task.FromResult(_authClient.User != null);
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

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            try
            {
                await _database
                    .Child("users")
                    .Child(user.Id)
                    .PutAsync(user);
                
                _currentUser = user;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Profile update failed: {ex.Message}");
            }
        }
    }
} 