using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TagHunt.Services;
using TagHunt.Services.Interfaces;

namespace TagHunt.ViewModels;

/// <summary>
/// ViewModel for the main dashboard/home screen that users see after login
/// Handles location sharing status, quick actions, and activity feed
/// </summary>
public class DashboardViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IFirebaseDbService _databaseService;
    private string _welcomeMessage = string.Empty;
    private string _userDisplayName = string.Empty;
    private string _userAvatarUrl = string.Empty;
    private bool _isLocationSharingActive;
    private string _locationSharingStatus = "Location sharing is off";
    private int _activeSharingSessionsCount;
    private bool _isLocationPermissionGranted;

    /// <summary>
    /// Initializes the dashboard view model with required services
    /// </summary>
    /// <param name="authService">Authentication service for user management</param>
    /// <param name="databaseService">Database service for real-time data</param>
    public DashboardViewModel(IAuthService authService, IFirebaseDbService databaseService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        
        // Initialize collections
        ActiveSessions = new ObservableCollection<LocationSharingSession>();
        RecentActivity = new ObservableCollection<ActivityItem>();
        
        // Initialize commands
        ShareLocationCommand = new Command(async () => await OnShareLocationAsync());
        FindFriendsCommand = new Command(async () => await OnFindFriendsAsync());
        ViewMapCommand = new Command(async () => await OnViewMapAsync());
        RefreshCommand = new Command(async () => await OnRefreshAsync());
        ViewProfileCommand = new Command(async () => await OnViewProfileAsync());
        
        // Load initial data
        _ = Task.Run(async () => await LoadDashboardDataAsync());
    }

    #region Properties

    /// <summary>
    /// Welcome message displayed to the user
    /// </summary>
    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set => SetProperty(ref _welcomeMessage, value);
    }

    /// <summary>
    /// Current user's display name
    /// </summary>
    public string UserDisplayName
    {
        get => _userDisplayName;
        set => SetProperty(ref _userDisplayName, value);
    }

    /// <summary>
    /// URL for user's avatar image
    /// </summary>
    public string UserAvatarUrl
    {
        get => _userAvatarUrl;
        set => SetProperty(ref _userAvatarUrl, value);
    }

    /// <summary>
    /// Indicates if location sharing is currently active
    /// </summary>
    public bool IsLocationSharingActive
    {
        get => _isLocationSharingActive;
        set => SetProperty(ref _isLocationSharingActive, value);
    }

    /// <summary>
    /// Status message for location sharing
    /// </summary>
    public string LocationSharingStatus
    {
        get => _locationSharingStatus;
        set => SetProperty(ref _locationSharingStatus, value);
    }

    /// <summary>
    /// Number of active location sharing sessions
    /// </summary>
    public int ActiveSharingSessionsCount
    {
        get => _activeSharingSessionsCount;
        set => SetProperty(ref _activeSharingSessionsCount, value);
    }

    /// <summary>
    /// Indicates if location permission has been granted
    /// </summary>
    public bool IsLocationPermissionGranted
    {
        get => _isLocationPermissionGranted;
        set => SetProperty(ref _isLocationPermissionGranted, value);
    }

    /// <summary>
    /// Collection of active location sharing sessions
    /// </summary>
    public ObservableCollection<LocationSharingSession> ActiveSessions { get; }

    /// <summary>
    /// Collection of recent activity items
    /// </summary>
    public ObservableCollection<ActivityItem> RecentActivity { get; }

    #endregion

    #region Commands

    /// <summary>
    /// Command to initiate location sharing
    /// </summary>
    public ICommand ShareLocationCommand { get; }

    /// <summary>
    /// Command to navigate to find friends screen
    /// </summary>
    public ICommand FindFriendsCommand { get; }

    /// <summary>
    /// Command to navigate to map view
    /// </summary>
    public ICommand ViewMapCommand { get; }

    /// <summary>
    /// Command to refresh dashboard data
    /// </summary>
    public ICommand RefreshCommand { get; }

    /// <summary>
    /// Command to view user profile
    /// </summary>
    public ICommand ViewProfileCommand { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Loads initial dashboard data including user info and active sessions
    /// </summary>
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsBusy = true;
            
            // Load user information
            await LoadUserInfoAsync();
            
            // Check location permission status
            await CheckLocationPermissionAsync();
            
            // Load active sharing sessions
            await LoadActiveSharingSessionsAsync();
            
            // Load recent activity
            await LoadRecentActivityAsync();
            
            // Update location sharing status
            UpdateLocationSharingStatus();
        }
        catch (Exception ex)
        {
            // Handle error appropriately
            await Shell.Current.DisplayAlert("Error", $"Failed to load dashboard data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Loads current user information from authentication service
    /// </summary>
    private async Task LoadUserInfoAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                UserDisplayName = currentUser.DisplayName ?? "User";
                UserAvatarUrl = ""; // TODO: Add avatar URL property to User model
                
                var timeOfDay = DateTime.Now.Hour switch
                {
                    < 12 => "Good morning",
                    < 17 => "Good afternoon",
                    _ => "Good evening"
                };
                
                WelcomeMessage = $"{timeOfDay}, {UserDisplayName}!";
            }
        }
        catch (Exception)
        {
            WelcomeMessage = "Welcome to TagHunt!";
            UserDisplayName = "User";
        }
    }

    /// <summary>
    /// Checks if location permission is granted
    /// </summary>
    private async Task CheckLocationPermissionAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            IsLocationPermissionGranted = status == PermissionStatus.Granted;
        }
        catch
        {
            IsLocationPermissionGranted = false;
        }
    }

    /// <summary>
    /// Loads active location sharing sessions from database
    /// </summary>
    private async Task LoadActiveSharingSessionsAsync()
    {
        try
        {
            // TODO: Implement when database service methods are available
            // For now, use mock data
            ActiveSessions.Clear();
            ActiveSharingSessionsCount = 0;
        }
        catch (Exception ex)
        {
            // Handle error
            await Shell.Current.DisplayAlert("Error", $"Failed to load active sessions: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Loads recent activity from database
    /// </summary>
    private async Task LoadRecentActivityAsync()
    {
        try
        {
            // TODO: Implement when database service methods are available
            // For now, use mock data
            RecentActivity.Clear();
            
            // Add mock activity items
            RecentActivity.Add(new ActivityItem
            {
                Title = "Welcome to TagHunt!",
                Description = "Start by adding friends and sharing your location",
                Timestamp = DateTime.Now,
                Type = ActivityType.System
            });
        }
        catch (Exception ex)
        {
            // Handle error
            await Shell.Current.DisplayAlert("Error", $"Failed to load recent activity: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Updates the location sharing status based on current state
    /// </summary>
    private void UpdateLocationSharingStatus()
    {
        if (!IsLocationPermissionGranted)
        {
            LocationSharingStatus = "Location permission required";
            IsLocationSharingActive = false;
        }
        else if (ActiveSharingSessionsCount > 0)
        {
            LocationSharingStatus = $"Sharing with {ActiveSharingSessionsCount} friend{(ActiveSharingSessionsCount == 1 ? "" : "s")}";
            IsLocationSharingActive = true;
        }
        else
        {
            LocationSharingStatus = "Location sharing is off";
            IsLocationSharingActive = false;
        }
    }

    /// <summary>
    /// Handles share location command
    /// </summary>
    private async Task OnShareLocationAsync()
    {
        try
        {
            if (!IsLocationPermissionGranted)
            {
                var result = await Shell.Current.DisplayAlert(
                    "Location Permission", 
                    "Location permission is required to share your location. Grant permission?", 
                    "Yes", "No");
                    
                if (result)
                {
                    var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    IsLocationPermissionGranted = status == PermissionStatus.Granted;
                    
                    if (!IsLocationPermissionGranted)
                    {
                        await Shell.Current.DisplayAlert("Permission Denied", "Location permission is required for sharing.", "OK");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            // Navigate to share location screen
            await Shell.Current.GoToAsync("//ShareLocationPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to start location sharing: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Handles find friends command
    /// </summary>
    private async Task OnFindFriendsAsync()
    {
        try
        {
            // Navigate to friends list screen
            await Shell.Current.GoToAsync("//FriendsListPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open friends list: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Handles view map command
    /// </summary>
    private async Task OnViewMapAsync()
    {
        try
        {
            // Navigate to map screen
            await Shell.Current.GoToAsync("//MapPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open map: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Handles refresh command
    /// </summary>
    private async Task OnRefreshAsync()
    {
        await LoadDashboardDataAsync();
    }



    /// <summary>
    /// Handles view profile command
    /// </summary>
    private async Task OnViewProfileAsync()
    {
        try
        {
            // Navigate to account settings screen
            await Shell.Current.GoToAsync("AccountSettingsPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open account settings: {ex.Message}", "OK");
        }
    }

    #endregion
}

/// <summary>
/// Represents a location sharing session
/// </summary>
public class LocationSharingSession
{
    public string Id { get; set; } = string.Empty;
    public string FriendName { get; set; } = string.Empty;
    public string FriendAvatarUrl { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ExpiresAt => StartTime.Add(Duration);
    public bool IsExpired => DateTime.Now > ExpiresAt;
    public TimeSpan TimeRemaining => IsExpired ? TimeSpan.Zero : ExpiresAt - DateTime.Now;
}

/// <summary>
/// Represents an activity item in the recent activity feed
/// </summary>
public class ActivityItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public ActivityType Type { get; set; }
    public string Icon => Type switch
    {
        ActivityType.LocationShare => "📍",
        ActivityType.FriendRequest => "👥",
        ActivityType.System => "ℹ️",
        _ => "•"
    };
}

/// <summary>
/// Types of activity items
/// </summary>
public enum ActivityType
{
    LocationShare,
    FriendRequest,
    System
} 