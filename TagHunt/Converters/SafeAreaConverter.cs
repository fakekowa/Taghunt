using Microsoft.Maui.Graphics;
using System.Globalization;
using TagHunt.Services.Interfaces;

namespace TagHunt.Converters
{
    public class SafeAreaConverter : IValueConverter
    {
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

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 