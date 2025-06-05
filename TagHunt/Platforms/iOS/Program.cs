using ObjCRuntime;
using UIKit;

namespace TagHunt;

/// <summary>
/// Main entry point for the iOS application
/// </summary>
public class Program
{
	/// <summary>
	/// Main entry point of the iOS application
	/// </summary>
	/// <param name="args">Command line arguments</param>
	static void Main(string[] args)
	{
		// Initialize the iOS application with the AppDelegate
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
