using TagHunt.Models;
using TagHunt.Services;
using TagHunt.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace TagHunt.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isLoggedIn;

        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            IsLoggedIn = await _authService.IsUserLoggedInAsync();
        }

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

                await _authService.LoginAsync(Email, Password);
                IsLoggedIn = true;

                // Clear sensitive data
                Email = string.Empty;
                Password = string.Empty;

                // Navigate to main page after successful login
                await Shell.Current.GoToAsync("//MainPage");
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

                await _authService.RegisterUserAsync(Email, Password, Username);
                IsLoggedIn = true;

                // Clear sensitive data
                Email = string.Empty;
                Password = string.Empty;
                Username = string.Empty;

                // Navigate to main page after successful registration
                await Shell.Current.GoToAsync("//MainPage");
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

        [RelayCommand]
        private async Task NavigateToRegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }

        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await Shell.Current.GoToAsync(".."); // Go back to login page
        }
    }
} 