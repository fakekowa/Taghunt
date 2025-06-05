using System.Globalization;

namespace TagHunt.Converters;

/// <summary>
/// Converts boolean values to colors for status indicators
/// True = Green (active), False = Red (inactive)
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    /// <summary>
    /// Converts boolean to color value
    /// </summary>
    /// <param name="value">Boolean value to convert</param>
    /// <param name="targetType">Target type (Color)</param>
    /// <param name="parameter">Optional parameter</param>
    /// <param name="culture">Culture info</param>
    /// <returns>Green color for true, red color for false</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Colors.Green : Colors.Red;
        }
        
        return Colors.Gray;
    }

    /// <summary>
    /// Converts color back to boolean (not implemented)
    /// </summary>
    /// <param name="value">Color value</param>
    /// <param name="targetType">Target type (bool)</param>
    /// <param name="parameter">Optional parameter</param>
    /// <param name="culture">Culture info</param>
    /// <returns>Not implemented</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("BoolToColorConverter does not support ConvertBack");
    }
} 