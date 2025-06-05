using TagHunt.Models;
using TagHunt.Services;
using TagHunt.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TagHunt.Services.Interfaces;

namespace TagHunt.ViewModels
{
    /// <summary>
    /// ViewModel that handles authentication operations including login, registration, and logout
    /// </summary>
    public partial class AuthViewModel : BaseViewModel
    {
        #region Fields

        private readonly IAuthService _authService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;

        #endregion

        #region Observable Properties

        /// <summary>
        /// Indicates whether an authentication operation is in progress
        /// </summary>
        [ObservableProperty]
        private bool isLoading;

        /// <summary>
        /// Indicates whether a user is currently logged in
        /// </summary>
        [ObservableProperty]
        private bool isLoggedIn;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Gets or sets the user's username
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// Gets or sets the current error message to display to the user
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AuthViewModel
        /// </summary>
        /// <param name="authService">The authentication service to use</param>
        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            CheckLoginStatus();
            
            // Test Firebase configuration in background 
            _ = Task.Run(async () => await TestFirebaseConfigurationAsync());
            
            // Auto-login for development/testing (only in debug mode)
#if DEBUG
            _ = Task.Run(async () => await AutoLoginAsync());
#endif
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tests Firebase configuration and displays helpful error messages
        /// </summary>
        private async Task TestFirebaseConfigurationAsync()
        {
            try
            {
                await _authService.TestConfigurationAsync();
                System.Diagnostics.Debug.WriteLine("Firebase configuration test successful");
                
                // Clear any existing error message on success
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (ErrorMessage.Contains("Firebase"))
                    {
                        ErrorMessage = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase configuration test failed: {ex.Message}");
                
                // Only show error if it's a real configuration issue, not a temporary network issue
                if (!ex.Message.Contains("network") && !ex.Message.Contains("timeout"))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ErrorMessage = "Firebase connection test failed. Please check your internet connection.";
                    });
                }
            }
        }

        /// <summary>
        /// Auto-login functionality for development/testing
        /// </summary>
        private async Task AutoLoginAsync()
        {
            try
            {
                // Wait a bit to let the UI load and configuration test complete
                await Task.Delay(2000);
                
                // Check if already logged in
                if (await _authService.IsUserLoggedInAsync())
                {
                    System.Diagnostics.Debug.WriteLine("User already logged in, skipping auto-login");
                    return;
                }
                
                // Development credentials for auto-login
                const string devEmail = "svedmanp@gmail.com";
                const string devPassword = "p0nTus5v";
                
                System.Diagnostics.Debug.WriteLine("Attempting auto-login for development...");
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Email = devEmail;
                    Password = devPassword;
                    ErrorMessage = "Auto-logging in...";
                });
                
                // Attempt login
                var user = await _authService.LoginAsync(devEmail, devPassword);
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsLoggedIn = true;
                    Email = string.Empty;
                    Password = string.Empty;
                    ErrorMessage = string.Empty;
                });
                
                System.Diagnostics.Debug.WriteLine($"Auto-login successful for user: {user.Id}");
                
                // Enable flyout and navigate to dashboard
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Shell.Current is AppShell appShell)
                    {
                        appShell.SetFlyoutEnabled(true);
                        appShell.UpdateUserInfo(user.DisplayName ?? "User", user.Email ?? "");
                    }
                    await Shell.Current.GoToAsync("Dashboard");
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Auto-login failed: {ex.Message}");
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ErrorMessage = $"Auto-login failed: {ex.Message}";
                    Email = string.Empty;
                    Password = string.Empty;
                });
            }
        }

        /// <summary>
        /// Checks the current login status asynchronously and auto-navigates if logged in
        /// </summary>
        private async void CheckLoginStatus()
        {
            try
            {
                // Give the auth service time to restore state
                await Task.Delay(500);
                
                IsLoggedIn = await _authService.IsUserLoggedInAsync();
                
                if (IsLoggedIn)
                {
                    // User is already logged in, enable flyout and navigate to dashboard
                    var currentUser = await _authService.GetCurrentUserAsync();
                    
                    if (Shell.Current is AppShell appShell)
                    {
                        if (currentUser != null)
                        {
                            appShell.UpdateUserInfo(currentUser.DisplayName ?? "User", currentUser.Email ?? "");
                        }
                        
                        // Ensure flyout is properly enabled before navigation
                        await appShell.EnsureFlyoutEnabledAsync();
                    }
                    
                    await Shell.Current.GoToAsync("//Dashboard");
                }
            }
            catch (Exception ex)
            {
                // If there's an error checking login status, just stay on login page
                System.Diagnostics.Debug.WriteLine($"Error checking login status: {ex.Message}");
                ErrorMessage = $"Login check failed: {ex.Message}";
                IsLoggedIn = false;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to handle user login
        /// </summary>
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please fill in all fields";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var user = await _authService.LoginAsync(Email, Password);
                IsLoggedIn = true;

                // Clear sensitive data
                Email = string.Empty;
                Password = string.Empty;

                // Enable flyout and navigate to dashboard page after successful login
                if (Shell.Current is AppShell appShell)
                {
                    appShell.UpdateUserInfo(user.DisplayName ?? "User", user.Email ?? "");
                    
                    // Ensure flyout is properly enabled before navigation
                    await appShell.EnsureFlyoutEnabledAsync();
                }
                await Shell.Current.GoToAsync("//Dashboard");
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                IsLoggedIn = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Command to handle user registration
        /// </summary>
        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please fill in all fields";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var user = await _authService.RegisterUserAsync(Email, Password, Username);
                IsLoggedIn = true;

                // Clear sensitive data
                Email = string.Empty;
                Password = string.Empty;
                Username = string.Empty;

                // Enable flyout and navigate to dashboard page after successful registration
                if (Shell.Current is AppShell appShell)
                {
                    appShell.UpdateUserInfo(user.DisplayName ?? "User", user.Email ?? "");
                    
                    // Ensure flyout is properly enabled before navigation
                    await appShell.EnsureFlyoutEnabledAsync();
                }
                await Shell.Current.GoToAsync("//Dashboard");
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
                IsLoggedIn = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Command to handle user logout
        /// </summary>
        [RelayCommand]
        private async Task LogoutAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _authService.LogoutAsync();
                IsLoggedIn = false;

                // Navigate to login page after logout
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Command to handle password reset
        /// </summary>
        [RelayCommand]
        private async Task ResetPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Please enter your email address";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _authService.ResetPasswordAsync(Email);
                ErrorMessage = "Password reset email sent. Please check your inbox.";
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Command to navigate to the registration page
        /// </summary>
        [RelayCommand]
        private async Task NavigateToRegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }

        /// <summary>
        /// Command to navigate to the login page
        /// </summary>
        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        #endregion
    }
} 