using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace TagHunt;

/// <summary>
/// Main activity for the Android platform that serves as the entry point for the MAUI application
/// </summary>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
