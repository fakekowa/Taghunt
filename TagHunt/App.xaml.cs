namespace TagHunt;

/// <summary>
/// Main application class that handles app initialization and window creation
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Initializes the application and its components
	/// </summary>
	public App()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Creates the main application window with the AppShell as the root page
	/// </summary>
	/// <param name="activationState">The activation state of the application</param>
	/// <returns>A new Window instance containing the AppShell</returns>
	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}