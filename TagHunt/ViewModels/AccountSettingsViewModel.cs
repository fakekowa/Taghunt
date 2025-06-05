using System.Windows.Input;
using TagHunt.Services.Interfaces;

namespace TagHunt.ViewModels;

/// <summary>
/// ViewModel for the account settings page where users can manage their profile
/// </summary>
public class AccountSettingsViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _displayName = string.Empty;
    private string _email = string.Empty;
    private string _username = string.Empty;
    private string _avatarUrl = string.Empty;
    private DateTime _createdAt;
    private DateTime _lastLoginAt;
    private bool _isOnline;
    private string _errorMessage = string.Empty;
    private string _successMessage = string.Empty;
    private bool _isEditing;

    /// <summary>
    /// Initializes the account settings view model
    /// </summary>
    /// <param name="authService">Authentication service for user management</param>
    public AccountSettingsViewModel(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        
        // Initialize commands
        EditProfileCommand = new Command(() => OnEditProfile());
        SaveProfileCommand = new Command(async () => await OnSaveProfileAsync(), () => !IsBusy && IsEditing);
        CancelEditCommand = new Command(() => OnCancelEdit());
        ChangeAvatarCommand = new Command(async () => await OnChangeAvatarAsync());
        BackCommand = new Command(async () => await OnBackAsync());
        
        // Load user data
        _ = Task.Run(async () => await LoadUserDataAsync());
    }

    #region Properties

    /// <summary>
    /// User's display name
    /// </summary>
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    /// <summary>
    /// User's username
    /// </summary>
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    /// <summary>
    /// URL for user's avatar image
    /// </summary>
    public string AvatarUrl
    {
        get => _avatarUrl;
        set => SetProperty(ref _avatarUrl, value);
    }

    /// <summary>
    /// Date when the account was created
    /// </summary>
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    /// <summary>
    /// Date of last login
    /// </summary>
    public DateTime LastLoginAt
    {
        get => _lastLoginAt;
        set => SetProperty(ref _lastLoginAt, value);
    }

    /// <summary>
    /// Whether the user is currently online
    /// </summary>
    public bool IsOnline
    {
        get => _isOnline;
        set => SetProperty(ref _isOnline, value);
    }

    /// <summary>
    /// Error message to display
    /// </summary>
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// Success message to display
    /// </summary>
    public string SuccessMessage
    {
        get => _successMessage;
        set => SetProperty(ref _successMessage, value);
    }

    /// <summary>
    /// Whether the user is currently editing their profile
    /// </summary>
    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            SetProperty(ref _isEditing, value);
            ((Command)SaveProfileCommand).ChangeCanExecute();
        }
    }

    /// <summary>
    /// Formatted account creation date
    /// </summary>
    public string FormattedCreatedAt => CreatedAt.ToString("MMMM dd, yyyy");

    /// <summary>
    /// Formatted last login date
    /// </summary>
    public string FormattedLastLoginAt => LastLoginAt.ToString("MMMM dd, yyyy 'at' HH:mm");

    /// <summary>
    /// Online status text
    /// </summary>
    public string OnlineStatusText => IsOnline ? "Online" : "Offline";

    /// <summary>
    /// Online status color
    /// </summary>
    public Color OnlineStatusColor => IsOnline ? Colors.Green : Colors.Gray;

    #endregion

    #region Commands

    /// <summary>
    /// Command to start editing profile
    /// </summary>
    public ICommand EditProfileCommand { get; }

    /// <summary>
    /// Command to save profile changes
    /// </summary>
    public ICommand SaveProfileCommand { get; }

    /// <summary>
    /// Command to cancel editing
    /// </summary>
    public ICommand CancelEditCommand { get; }

    /// <summary>
    /// Command to change avatar
    /// </summary>
    public ICommand ChangeAvatarCommand { get; }

    /// <summary>
    /// Command to go back
    /// </summary>
    public ICommand BackCommand { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Loads current user data from the authentication service
    /// </summary>
    private async Task LoadUserDataAsync()
    {
        try
        {
            IsBusy = true;
            ClearMessages();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                DisplayName = currentUser.DisplayName ?? "";
                Email = currentUser.Email ?? "";
                Username = currentUser.Username ?? "";
                AvatarUrl = ""; // TODO: Add avatar URL to User model
                CreatedAt = currentUser.CreatedAt;
                LastLoginAt = currentUser.LastLoginAt;
                IsOnline = currentUser.IsOnline;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load user data: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Starts profile editing mode
    /// </summary>
    private void OnEditProfile()
    {
        IsEditing = true;
        ClearMessages();
    }

    /// <summary>
    /// Saves profile changes
    /// </summary>
    private async Task OnSaveProfileAsync()
    {
        try
        {
            IsBusy = true;
            ClearMessages();

            // Validate input
            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                ErrorMessage = "Display name cannot be empty";
                return;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Username cannot be empty";
                return;
            }

            // Get current user and update
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                currentUser.DisplayName = DisplayName.Trim();
                currentUser.Username = Username.Trim();
                
                await _authService.UpdateUserProfileAsync(currentUser);
                
                // Update AppShell user info
                if (Shell.Current is AppShell appShell)
                {
                    appShell.UpdateUserInfo(DisplayName, Email);
                }
                
                SuccessMessage = "Profile updated successfully!";
                IsEditing = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update profile: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Cancels profile editing
    /// </summary>
    private void OnCancelEdit()
    {
        IsEditing = false;
        ClearMessages();
        
        // Reload data to reset any changes
        _ = Task.Run(async () => await LoadUserDataAsync());
    }

    /// <summary>
    /// Changes user avatar
    /// </summary>
    private async Task OnChangeAvatarAsync()
    {
        try
        {
            // TODO: Implement avatar selection and upload
            await Shell.Current.DisplayAlert("Feature Coming Soon", "Avatar selection will be available in a future update.", "OK");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to change avatar: {ex.Message}";
        }
    }

    /// <summary>
    /// Navigates back to the previous page
    /// </summary>
    private async Task OnBackAsync()
    {
        try
        {
            if (IsEditing)
            {
                var result = await Shell.Current.DisplayAlert(
                    "Unsaved Changes", 
                    "You have unsaved changes. Are you sure you want to go back?", 
                    "Yes", "No");
                    
                if (!result)
                {
                    return;
                }
            }

            await Shell.Current.GoToAsync("///Dashboard");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to navigate back: {ex.Message}";
        }
    }

    /// <summary>
    /// Clears all messages
    /// </summary>
    private void ClearMessages()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }

    #endregion
} 