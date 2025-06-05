using TagHunt.Views;

namespace TagHunt;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	private void RegisterRoutes()
	{
		Routing.RegisterRoute("LoginPage", typeof(LoginPage));
		Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
		Routing.RegisterRoute("MainPage", typeof(MainPage));
	}
}
