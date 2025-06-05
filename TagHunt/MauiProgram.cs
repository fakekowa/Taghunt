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

public static class MauiProgram
{
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

		// Add configuration
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("TagHunt.appsettings.json");
		if (stream != null)
		{
			var config = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();
			builder.Configuration.AddConfiguration(config);
		}

		// Register services
		builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
		builder.Services.AddSingleton<FirebaseClient>();

		builder.Services.AddSingleton<IAuthService>(provider =>
		{
			var configService = provider.GetRequiredService<IConfigurationService>();
			var config = configService.GetFirebaseConfig();

			return new FirebaseAuthService(config.ApiKey, config.AuthDomain);
		});

		// Register ViewModels
		builder.Services.AddSingleton<AuthViewModel>();

		// Register Pages
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
