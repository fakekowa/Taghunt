using TagHunt.Views;

namespace TagHunt;

/// <summary>
/// Application shell that manages navigation and routing for the app
/// </summary>
public partial class AppShell : Shell
{
	/// <summary>
	/// Initializes the AppShell and registers navigation routes
	/// </summary>
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	/// <summary>
	/// Registers all navigation routes for the application pages
	/// </summary>
	private void RegisterRoutes()
	{
		Routing.RegisterRoute("LoginPage", typeof(LoginPage));
		Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
		Routing.RegisterRoute("MainPage", typeof(MainPage));
	}
}
