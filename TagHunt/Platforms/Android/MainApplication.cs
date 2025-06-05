using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TagHunt;

/// <summary>
/// Main application class for Android that initializes the MAUI application
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
	/// <summary>
	/// Initializes a new instance of the MainApplication class
	/// </summary>
	/// <param name="handle">The JNI handle</param>
	/// <param name="ownership">The JNI handle ownership</param>
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	/// <summary>
	/// Creates the MAUI application instance
	/// </summary>
	/// <returns>The configured MAUI application</returns>
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
