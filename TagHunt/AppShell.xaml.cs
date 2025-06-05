using System.Windows.Input;
using TagHunt.Services.Interfaces;
using TagHunt.Views;

namespace TagHunt;

/// <summary>
/// Application shell that manages navigation and routing for the app
/// </summary>
public partial class AppShell : Shell
{
	private IAuthService? _authService;

	/// <summary>
	/// Command to navigate to account settings
	/// </summary>
	public ICommand AccountSettingsCommand { get; }

	/// <summary>
	/// Command to logout user
	/// </summary>
	public ICommand LogoutCommand { get; }

	/// <summary>
	/// Initializes the AppShell and registers navigation routes
	/// </summary>
	public AppShell()
	{
		InitializeComponent();
		
		// Initialize commands
		AccountSettingsCommand = new Command(async () => await OnAccountSettingsAsync());
		LogoutCommand = new Command(async () => await OnLogoutAsync());

		RegisterRoutes();
		
		// Set binding context to self for command binding
		BindingContext = this;
	}

	/// <summary>
	/// Called when the shell is loaded - sets up auth service and loads user info
	/// </summary>
	protected override void OnNavigated(ShellNavigatedEventArgs args)
	{
		base.OnNavigated(args);
		
		if (_authService == null)
		{
			try
			{
				_authService = Handler?.MauiContext?.Services?.GetService<IAuthService>();
				if (_authService != null)
				{
					LoadUserInfo();
				}
			}
			catch
			{
				// Ignore errors during initialization
			}
		}
	}

	/// <summary>
	/// Registers all navigation routes for the application pages
	/// </summary>
	private void RegisterRoutes()
	{
		Routing.RegisterRoute("LoginPage", typeof(LoginPage));
		Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
		Routing.RegisterRoute("AccountSettingsPage", typeof(AccountSettingsPage));
		Routing.RegisterRoute("Dashboard", typeof(DashboardPage));
		Routing.RegisterRoute("TestPage", typeof(TestPage));
		Routing.RegisterRoute("Friends", typeof(DashboardPage));
		Routing.RegisterRoute("Map", typeof(DashboardPage));
	}

	/// <summary>
	/// Loads and displays current user information in the flyout header
	/// </summary>
	private async void LoadUserInfo()
	{
		try
		{
			if (_authService != null)
			{
				var currentUser = await _authService.GetCurrentUserAsync();
				if (currentUser != null)
				{
					FlyoutUserName.Text = currentUser.DisplayName ?? "User";
					FlyoutUserEmail.Text = currentUser.Email ?? "";
					return;
				}
			}
		}
		catch
		{
			// Handle error silently - use default values
		}

		// Default values
		FlyoutUserName.Text = "Welcome to TagHunt";
		FlyoutUserEmail.Text = "";
	}

	/// <summary>
	/// Updates the flyout header with current user information
	/// </summary>
	public void UpdateUserInfo(string displayName, string email)
	{
		FlyoutUserName.Text = displayName ?? "User";
		FlyoutUserEmail.Text = email ?? "";
	}

	/// <summary>
	/// Enables or disables the flyout menu
	/// </summary>
	/// <param name="enabled">True to enable flyout, false to disable</param>
	public void SetFlyoutEnabled(bool enabled)
	{
		FlyoutBehavior = enabled ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;
	}

	/// <summary>
	/// Handles account settings command
	/// </summary>
	private async Task OnAccountSettingsAsync()
	{
		try
		{
			FlyoutIsPresented = false; // Close the flyout
			await Shell.Current.GoToAsync("AccountSettingsPage");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to open account settings: {ex.Message}", "OK");
		}
	}

	/// <summary>
	/// Handles logout command
	/// </summary>
	private async Task OnLogoutAsync()
	{
		try
		{
			var result = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
			if (result)
			{
				FlyoutIsPresented = false; // Close the flyout
				
				// Logout user
				if (_authService != null)
				{
					await _authService.LogoutAsync();
				}
				
				// Disable flyout and navigate to login
				SetFlyoutEnabled(false);
				await Shell.Current.GoToAsync("//LoginPage");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to logout: {ex.Message}", "OK");
		}
	}
}
