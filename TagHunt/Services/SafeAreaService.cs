using Microsoft.Maui.Graphics;
using TagHunt.Services.Interfaces;
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
    /// <summary>
    /// Service that provides safe area insets and dynamic padding for different platforms
    /// </summary>
    public class SafeAreaService : ISafeAreaService
    {
        /// <summary>
        /// Gets the safe area insets for the current platform
        /// </summary>
        /// <returns>Safe area insets as Thickness</returns>
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

        /// <summary>
        /// Gets dynamic padding that respects safe areas while maintaining minimum values
        /// </summary>
        /// <param name="minimumHorizontal">Minimum horizontal padding</param>
        /// <param name="minimumVertical">Minimum vertical padding</param>
        /// <returns>Dynamic padding as Thickness</returns>
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
        /// <summary>
        /// Gets safe area insets for Android devices, including notch/cutout handling
        /// </summary>
        /// <returns>Android safe area insets</returns>
        private Thickness GetAndroidSafeArea()
        {
            var context = Platform.CurrentActivity ?? Android.App.Application.Context;
            if (context == null)
            {
                return new Thickness(20);
            }

            // Get display metrics
            var displayMetrics = context.Resources?.DisplayMetrics;
            if (displayMetrics == null)
            {
                return new Thickness(20);
            }

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
        /// <summary>
        /// Gets safe area insets for iOS devices
        /// </summary>
        /// <returns>iOS safe area insets</returns>
        private Thickness GetIOSSafeArea()
        {
            var window = GetKeyWindow();
            if (window == null)
            {
                return new Thickness(20, 44, 20, 34); // Default for iPhone X+ style
            }

            var safeAreaInsets = window.SafeAreaInsets;
            
            // Convert from iOS points to MAUI units
            return new Thickness(
                Math.Max(20, safeAreaInsets.Left),
                Math.Max(20, safeAreaInsets.Top),
                Math.Max(20, safeAreaInsets.Right),
                Math.Max(20, safeAreaInsets.Bottom)
            );
        }

        /// <summary>
        /// Gets the key window for iOS, handling different iOS versions
        /// </summary>
        /// <returns>The key UIWindow or null if not found</returns>
        private UIKit.UIWindow? GetKeyWindow()
        {
            // Use ConnectedScenes only on iOS 13+
            if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                try
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    var connectedScenes = UIKit.UIApplication.SharedApplication.ConnectedScenes;
                    var windowScene = connectedScenes.ToArray().OfType<UIKit.UIWindowScene>().FirstOrDefault();
                    if (windowScene != null)
                    {
                        return windowScene.Windows.FirstOrDefault(w => w.IsKeyWindow);
                    }
#pragma warning restore CA1416 // Validate platform compatibility
                }
                catch
                {
                    // Fall through to legacy approach
                }
            }
            
            // Fallback for iOS 11-12 or if ConnectedScenes approach fails
#pragma warning disable CA1422 // Validate platform compatibility
            return UIKit.UIApplication.SharedApplication.KeyWindow;
#pragma warning restore CA1422 // Validate platform compatibility
        }
#endif
    }
} 