using Microsoft.Maui.Graphics;
#if IOS
using UIKit;
#endif

namespace TagHunt.Behaviors
{
    /// <summary>
    /// Behavior that automatically applies safe area padding to layouts to avoid system UI elements
    /// </summary>
    public class SafeAreaBehavior : Behavior<Layout>
    {
        #region Bindable Properties
        
        /// <summary>
        /// Bindable property to control whether top safe area should be applied
        /// </summary>
        public static readonly BindableProperty UseTopProperty = 
            BindableProperty.Create(nameof(UseTop), typeof(bool), typeof(SafeAreaBehavior), true);
        
        /// <summary>
        /// Bindable property to control whether bottom safe area should be applied
        /// </summary>
        public static readonly BindableProperty UseBottomProperty = 
            BindableProperty.Create(nameof(UseBottom), typeof(bool), typeof(SafeAreaBehavior), true);
        
        /// <summary>
        /// Bindable property to control whether left safe area should be applied
        /// </summary>
        public static readonly BindableProperty UseLeftProperty = 
            BindableProperty.Create(nameof(UseLeft), typeof(bool), typeof(SafeAreaBehavior), true);
        
        /// <summary>
        /// Bindable property to control whether right safe area should be applied
        /// </summary>
        public static readonly BindableProperty UseRightProperty = 
            BindableProperty.Create(nameof(UseRight), typeof(bool), typeof(SafeAreaBehavior), true);
        
        /// <summary>
        /// Bindable property for minimum padding value
        /// </summary>
        public static readonly BindableProperty MinimumPaddingProperty = 
            BindableProperty.Create(nameof(MinimumPadding), typeof(double), typeof(SafeAreaBehavior), 20.0);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether to apply top safe area padding
        /// </summary>
        public bool UseTop
        {
            get => (bool)GetValue(UseTopProperty);
            set => SetValue(UseTopProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to apply bottom safe area padding
        /// </summary>
        public bool UseBottom
        {
            get => (bool)GetValue(UseBottomProperty);
            set => SetValue(UseBottomProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to apply left safe area padding
        /// </summary>
        public bool UseLeft
        {
            get => (bool)GetValue(UseLeftProperty);
            set => SetValue(UseLeftProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to apply right safe area padding
        /// </summary>
        public bool UseRight
        {
            get => (bool)GetValue(UseRightProperty);
            set => SetValue(UseRightProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum padding to apply
        /// </summary>
        public double MinimumPadding
        {
            get => (double)GetValue(MinimumPaddingProperty);
            set => SetValue(MinimumPaddingProperty, value);
        }

        #endregion

        /// <summary>
        /// Called when the behavior is attached to a layout
        /// </summary>
        /// <param name="bindable">The layout to attach to</param>
        protected override void OnAttachedTo(Layout bindable)
        {
            base.OnAttachedTo(bindable);
            SetSafeAreaPadding(bindable);
        }

        /// <summary>
        /// Calculates and applies safe area padding to the layout
        /// </summary>
        /// <param name="layout">The layout to apply padding to</param>
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

        /// <summary>
        /// Gets the safe area insets for the current platform
        /// </summary>
        /// <returns>Safe area insets as Thickness</returns>
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
        /// <summary>
        /// Gets safe area insets for iOS devices
        /// </summary>
        /// <returns>iOS safe area insets</returns>
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
        /// <summary>
        /// Gets safe area insets for Android devices
        /// </summary>
        /// <returns>Android safe area insets</returns>
        private Thickness GetAndroidSafeAreaInsets()
        {
            // For Android, we generally use standard padding as the system handles safe areas
            return new Thickness(0, 0, 0, 0);
        }
#endif
    }
} 