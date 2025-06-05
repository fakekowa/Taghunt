using Microsoft.Extensions.Logging;
using TagHunt.Services;
using TagHunt.ViewModels;
using TagHunt.Views;
using Firebase.Auth;
using Firebase.Database;
using TagHunt.Services.Interfaces;

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

		// Register services
		builder.Services.AddSingleton<FirebaseAuthClient>();
		builder.Services.AddSingleton<FirebaseClient>();
		builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();

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
