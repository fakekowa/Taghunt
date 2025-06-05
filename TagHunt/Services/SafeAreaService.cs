using Microsoft.Maui.Graphics;
using TagHunt.Services.Interfaces;
using System.Linq;
#if IOS
using UIKit;
using Foundation;
#endif
#if ANDROID
using Android.App;
using Android.Content;
using AndroidX.Core.View;
#endif

namespace TagHunt.Services
{
    public class SafeAreaService : ISafeAreaService
    {
        public Thickness GetSafeAreaInsets()
        {
#if ANDROID
            return GetAndroidSafeArea();
#elif IOS
            return GetIOSSafeArea();
#else
            return new Thickness(20); // Default fallback
#endif
        }

        public Thickness GetDynamicPadding(double minimumHorizontal = 20, double minimumVertical = 20)
        {
            var safeArea = GetSafeAreaInsets();
            
            return new Thickness(
                Math.Max(minimumHorizontal, safeArea.Left),
                Math.Max(minimumVertical, safeArea.Top),
                Math.Max(minimumHorizontal, safeArea.Right),
                Math.Max(minimumVertical, safeArea.Bottom)
            );
        }

#if ANDROID
        private Thickness GetAndroidSafeArea()
        {
            var context = Platform.CurrentActivity ?? Android.App.Application.Context;
            if (context == null) return new Thickness(20);

            // Get display metrics
            var displayMetrics = context.Resources?.DisplayMetrics;
            if (displayMetrics == null) return new Thickness(20);

            // For Android, we typically just need standard padding
            // as the system handles most safe area concerns
            var density = displayMetrics.Density;
            var standardPadding = 20 * density;

            // Check for notch/cutout on Android API 28+
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
            {
                if (context is Android.App.Activity activity && activity.Window != null)
                {
                    var windowInsets = activity.Window.DecorView.RootWindowInsets;
                    if (windowInsets != null)
                    {
                        var cutout = windowInsets.DisplayCutout;
                        if (cutout != null)
                        {
                            return new Thickness(
                                Math.Max(standardPadding / density, cutout.SafeInsetLeft / density),
                                Math.Max(standardPadding / density, cutout.SafeInsetTop / density),
                                Math.Max(standardPadding / density, cutout.SafeInsetRight / density),
                                Math.Max(standardPadding / density, cutout.SafeInsetBottom / density)
                            );
                        }
                    }
                }
            }

            return new Thickness(20, 20, 20, 20);
        }
#endif

#if IOS
        private Thickness GetIOSSafeArea()
        {
            var window = GetKeyWindow();
            if (window == null) return new Thickness(20, 44, 20, 34); // Default for iPhone X+ style

            var safeAreaInsets = window.SafeAreaInsets;
            
            // Convert from iOS points to MAUI units
            return new Thickness(
                Math.Max(20, safeAreaInsets.Left),
                Math.Max(20, safeAreaInsets.Top),
                Math.Max(20, safeAreaInsets.Right),
                Math.Max(20, safeAreaInsets.Bottom)
            );
        }

        private UIKit.UIWindow? GetKeyWindow()
        {
            // Use ConnectedScenes only on iOS 13+
            if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                var connectedScenes = UIKit.UIApplication.SharedApplication.ConnectedScenes;
                var windowScene = connectedScenes.ToArray().OfType<UIKit.UIWindowScene>().FirstOrDefault();
                if (windowScene != null)
                {
                    return windowScene.Windows.FirstOrDefault(w => w.IsKeyWindow);
                }
            }
            
            // Fallback for iOS 11-12 or if ConnectedScenes approach fails
            return UIKit.UIApplication.SharedApplication.KeyWindow;
        }
#endif
    }
} 