using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services
{
    /// <summary>
    /// Mock authentication service for testing purposes
    /// </summary>
    public class MockAuthService : IAuthService
    {
        private Models.User? _currentUser;
        private bool _isLoggedIn;

        /// <summary>
        /// Mock registration - always succeeds
        /// </summary>
        public async Task<Models.User> RegisterUserAsync(string email, string password, string username)
        {
            await Task.Delay(500); // Simulate network delay
            
            _currentUser = new Models.User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                DisplayName = username,
                Username = username,
                CreatedAt = DateTime.UtcNow
            };
            _isLoggedIn = true;
            
            return _currentUser;
        }

        /// <summary>
        /// Mock login - always succeeds
        /// </summary>
        public async Task<Models.User> LoginAsync(string email, string password)
        {
            await Task.Delay(500); // Simulate network delay
            
            _currentUser = new Models.User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                DisplayName = "Test User",
                Username = "testuser",
                CreatedAt = DateTime.UtcNow
            };
            _isLoggedIn = true;
            
            return _currentUser;
        }

        /// <summary>
        /// Returns the mock current user
        /// </summary>
        public async Task<Models.User?> GetCurrentUserAsync()
        {
            await Task.Delay(100);
            return _isLoggedIn ? _currentUser : null;
        }

        /// <summary>
        /// Mock profile update
        /// </summary>
        public async Task UpdateUserProfileAsync(Models.User user)
        {
            await Task.Delay(300);
            if (_isLoggedIn && _currentUser != null)
            {
                _currentUser.DisplayName = user.DisplayName;
                _currentUser.Username = user.Username;
            }
        }

        /// <summary>
        /// Mock logout
        /// </summary>
        public async Task LogoutAsync()
        {
            await Task.Delay(200);
            _currentUser = null;
            _isLoggedIn = false;
        }

        /// <summary>
        /// Returns mock login status
        /// </summary>
        public async Task<bool> IsUserLoggedInAsync()
        {
            await Task.Delay(100);
            return _isLoggedIn;
        }

        /// <summary>
        /// Mock password reset - always succeeds
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email)
        {
            await Task.Delay(400);
            return true; // Always successful in mock
        }

        /// <summary>
        /// Mock configuration test - always succeeds
        /// </summary>
        public async Task<bool> TestConfigurationAsync()
        {
            await Task.Delay(200);
            return true; // Always successful in mock
        }
    }
} 