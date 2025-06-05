using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TagHunt;

/// <summary>
/// iOS application delegate that initializes the MAUI application
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	/// <summary>
	/// Creates the MAUI application instance
	/// </summary>
	/// <returns>The configured MAUI application</returns>
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
