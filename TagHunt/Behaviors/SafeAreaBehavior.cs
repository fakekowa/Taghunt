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
            BindableProperty.Create(nameof(MinimumPadding), typeof(double), typeof(SafeAreaBehavior), 10.0);

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
            return new Thickness(0);
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
#pragma warning disable CA1416 // Validate platform compatibility
                    var connectedScenes = UIKit.UIApplication.SharedApplication.ConnectedScenes;
                    var windowScene = connectedScenes.ToArray().OfType<UIKit.UIWindowScene>().FirstOrDefault();
                    if (windowScene != null)
                    {
                        window = windowScene.Windows.FirstOrDefault(w => w.IsKeyWindow);
                    }
#pragma warning restore CA1416 // Validate platform compatibility
                }
                else
                {
                    // Fallback for iOS 11-12
#pragma warning disable CA1422 // Validate platform compatibility
                    window = UIKit.UIApplication.SharedApplication.KeyWindow;
#pragma warning restore CA1422 // Validate platform compatibility
                }

                if (window?.SafeAreaInsets != null)
                {
                    var insets = window.SafeAreaInsets;
                    // Only apply meaningful safe area values - avoid unnecessary padding
                    return new Thickness(
                        insets.Left > 0 ? insets.Left : 0,
                        insets.Top > 0 ? insets.Top : 0,
                        insets.Right > 0 ? insets.Right : 0,
                        insets.Bottom > 0 ? insets.Bottom : 0
                    );
                }
            }
            catch
            {
                // Fallback if we can't get safe area
            }
            
            // Return zero padding for devices without safe area needs
            return new Thickness(0);
        }
#endif

#if ANDROID
        /// <summary>
        /// Gets safe area insets for Android devices
        /// </summary>
        /// <returns>Android safe area insets</returns>
        private Thickness GetAndroidSafeAreaInsets()
        {
            // For Android, we generally don't need safe area padding as the system handles it
            // Only apply minimal padding where absolutely necessary
            return new Thickness(0);
        }
#endif
    }
} 