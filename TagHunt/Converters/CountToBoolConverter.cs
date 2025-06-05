using System.Globalization;

namespace TagHunt.Converters;

/// <summary>
/// Converts count values to boolean for visibility binding
/// Count > 0 = True (visible), Count = 0 = False (hidden)
/// </summary>
public class CountToBoolConverter : IValueConverter
{
    /// <summary>
    /// Converts count to boolean value
    /// </summary>
    /// <param name="value">Count value to convert</param>
    /// <param name="targetType">Target type (bool)</param>
    /// <param name="parameter">Optional parameter</param>
    /// <param name="culture">Culture info</param>
    /// <returns>True if count > 0, false otherwise</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
        {
            return count > 0;
        }
        
        if (value is double doubleCount)
        {
            return doubleCount > 0;
        }
        
        if (value is float floatCount)
        {
            return floatCount > 0;
        }
        
        return false;
    }

    /// <summary>
    /// Converts boolean back to count (not implemented)
    /// </summary>
    /// <param name="value">Boolean value</param>
    /// <param name="targetType">Target type (int)</param>
    /// <param name="parameter">Optional parameter</param>
    /// <param name="culture">Culture info</param>
    /// <returns>Not implemented</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("CountToBoolConverter does not support ConvertBack");
    }
} 