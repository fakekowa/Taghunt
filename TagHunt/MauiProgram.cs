using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using TagHunt.Services;
using TagHunt.ViewModels;
using TagHunt.Views;
using Firebase.Auth;
using Firebase.Database;
using TagHunt.Services.Interfaces;
using System.Reflection;

namespace TagHunt;

/// <summary>
/// Main program class that configures and builds the MAUI application
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// Creates and configures the MAUI application with all necessary services and dependencies
	/// </summary>
	/// <returns>A configured MauiApp instance</returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Add configuration from embedded appsettings.json
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("TagHunt.appsettings.json");
		if (stream != null)
		{
			var config = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();
			builder.Configuration.AddConfiguration(config);
		}

		// Register core services
		builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
		builder.Services.AddSingleton<FirebaseClient>();

		// Register authentication service with Firebase configuration
		builder.Services.AddSingleton<IAuthService>(provider =>
		{
			var configService = provider.GetRequiredService<IConfigurationService>();
			var config = configService.GetFirebaseConfig();

			return new FirebaseAuthService(config.ApiKey, config.AuthDomain);
		});

		// Register ViewModels for dependency injection
		builder.Services.AddSingleton<AuthViewModel>();

		// Register Pages for dependency injection
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();

#if DEBUG
		// Add debug logging in development builds
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
