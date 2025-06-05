using TagHunt.Models;
using TagHunt.Services;
using TagHunt.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TagHunt.Services.Interfaces;

namespace TagHunt.ViewModels
{
    public partial class AuthViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isLoggedIn;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

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
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
} 