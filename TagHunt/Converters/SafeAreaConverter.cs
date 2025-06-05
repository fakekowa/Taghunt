using Microsoft.Maui.Graphics;
using System.Globalization;
using TagHunt.Services.Interfaces;

namespace TagHunt.Converters
{
    /// <summary>
    /// Value converter that provides safe area padding for UI elements
    /// </summary>
    public class SafeAreaConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value to safe area padding by retrieving it from the safe area service
        /// </summary>
        /// <param name="value">The input value (not used)</param>
        /// <param name="targetType">The target type for conversion</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Thickness representing safe area padding</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // This will be called when the page is loaded
            // We'll get the safe area service from the current app's service provider
            var serviceProvider = IPlatformApplication.Current?.Services;
            if (serviceProvider != null)
            {
                var safeAreaService = serviceProvider.GetService<ISafeAreaService>();
                if (safeAreaService != null)
                {
                    return safeAreaService.GetDynamicPadding();
                }
            }
            
            // Fallback to default padding
            return new Thickness(20);
        }

        /// <summary>
        /// Converts back from safe area padding (not implemented as it's not needed)
        /// </summary>
        /// <param name="value">The value to convert back</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Throws NotImplementedException</returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 