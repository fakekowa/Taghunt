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
        /// Checks the current login status asynchronously and auto-navigates if logged in
        /// </summary>
        private async void CheckLoginStatus()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("CheckLoginStatus: Starting login status check...");
                
                // The FirebaseAuthService now properly awaits restoration in IsUserLoggedInAsync()
                // No need for arbitrary delays here
                IsLoggedIn = await _authService.IsUserLoggedInAsync();
                
                System.Diagnostics.Debug.WriteLine($"CheckLoginStatus: IsLoggedIn = {IsLoggedIn}");
                
                if (IsLoggedIn)
                {
                    System.Diagnostics.Debug.WriteLine("CheckLoginStatus: User is logged in, getting user data...");
                    
                    // User is already logged in, enable flyout and navigate to dashboard
                    var currentUser = await _authService.GetCurrentUserAsync();
                    
                    if (Shell.Current is AppShell appShell)
                    {
                        if (currentUser != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"CheckLoginStatus: Updating UI for user {currentUser.Email}");
                            appShell.UpdateUserInfo(currentUser.DisplayName ?? "User", currentUser.Email ?? "");
                        }
                        
                        // Ensure flyout is properly enabled before navigation
                        await appShell.EnsureFlyoutEnabledAsync();
                    }
                    
                    System.Diagnostics.Debug.WriteLine("CheckLoginStatus: Navigating to Dashboard...");
                    await Shell.Current.GoToAsync("//Dashboard");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("CheckLoginStatus: User not logged in, staying on login page");
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