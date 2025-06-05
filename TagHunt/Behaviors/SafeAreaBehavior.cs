using Microsoft.Maui.Graphics;
using System.Linq;
#if IOS
using UIKit;
#endif

namespace TagHunt.Behaviors
{
    public class SafeAreaBehavior : Behavior<Layout>
    {
        public static readonly BindableProperty UseTopProperty = 
            BindableProperty.Create(nameof(UseTop), typeof(bool), typeof(SafeAreaBehavior), true);
        
        public static readonly BindableProperty UseBottomProperty = 
            BindableProperty.Create(nameof(UseBottom), typeof(bool), typeof(SafeAreaBehavior), true);
        
        public static readonly BindableProperty UseLeftProperty = 
            BindableProperty.Create(nameof(UseLeft), typeof(bool), typeof(SafeAreaBehavior), true);
        
        public static readonly BindableProperty UseRightProperty = 
            BindableProperty.Create(nameof(UseRight), typeof(bool), typeof(SafeAreaBehavior), true);
        
        public static readonly BindableProperty MinimumPaddingProperty = 
            BindableProperty.Create(nameof(MinimumPadding), typeof(double), typeof(SafeAreaBehavior), 20.0);

        public bool UseTop
        {
            get => (bool)GetValue(UseTopProperty);
            set => SetValue(UseTopProperty, value);
        }

        public bool UseBottom
        {
            get => (bool)GetValue(UseBottomProperty);
            set => SetValue(UseBottomProperty, value);
        }

        public bool UseLeft
        {
            get => (bool)GetValue(UseLeftProperty);
            set => SetValue(UseLeftProperty, value);
        }

        public bool UseRight
        {
            get => (bool)GetValue(UseRightProperty);
            set => SetValue(UseRightProperty, value);
        }

        public double MinimumPadding
        {
            get => (double)GetValue(MinimumPaddingProperty);
            set => SetValue(MinimumPaddingProperty, value);
        }

        protected override void OnAttachedTo(Layout bindable)
        {
            base.OnAttachedTo(bindable);
            SetSafeAreaPadding(bindable);
        }

        private void SetSafeAreaPadding(Layout layout)
        {
            var safeAreaInsets = GetSafeAreaInsets();
            var minPadding = MinimumPadding;

            var padding = new Thickness(
                UseLeft ? Math.Max(minPadding, safeAreaInsets.Left) : minPadding,
                UseTop ? Math.Max(minPadding, safeAreaInsets.Top) : minPadding,
                UseRight ? Math.Max(minPadding, safeAreaInsets.Right) : minPadding,
                UseBottom ? Math.Max(minPadding, safeAreaInsets.Bottom) : minPadding
            );

            layout.Padding = padding;
        }

        private Thickness GetSafeAreaInsets()
        {
#if IOS
            return GetIOSSafeAreaInsets();
#elif ANDROID
            return GetAndroidSafeAreaInsets();
#else
            return new Thickness(MinimumPadding);
#endif
        }

#if IOS
        private Thickness GetIOSSafeAreaInsets()
        {
            try
            {
                UIKit.UIWindow? window = null;
                
                // Use ConnectedScenes only on iOS 13+
                if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    var connectedScenes = UIKit.UIApplication.SharedApplication.ConnectedScenes;
                    var windowScene = connectedScenes.ToArray().OfType<UIKit.UIWindowScene>().FirstOrDefault();
                    if (windowScene != null)
                    {
                        window = windowScene.Windows.FirstOrDefault(w => w.IsKeyWindow);
                    }
                }
                else
                {
                    // Fallback for iOS 11-12
                    window = UIKit.UIApplication.SharedApplication.KeyWindow;
                }

                if (window?.SafeAreaInsets != null)
                {
                    var insets = window.SafeAreaInsets;
                    return new Thickness(insets.Left, insets.Top, insets.Right, insets.Bottom);
                }
            }
            catch
            {
                // Fallback if we can't get safe area
            }
            
            // Default iOS safe area values for iPhone X+ style devices
            return new Thickness(0, 44, 0, 34);
        }
#endif

#if ANDROID
        private Thickness GetAndroidSafeAreaInsets()
        {
            // For Android, we generally use standard padding as the system handles safe areas
            return new Thickness(0, 0, 0, 0);
        }
#endif
    }
} 