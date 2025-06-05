using System.Globalization;

namespace TagHunt.Converters
{
    /// <summary>
    /// Value converter that inverts boolean values (true becomes false, false becomes true)
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to its inverse
        /// </summary>
        /// <param name="value">The boolean value to convert</param>
        /// <param name="targetType">The target type for conversion</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>The inverted boolean value or null if input is not boolean</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return null;
        }

        /// <summary>
        /// Converts back from an inverted boolean value to the original
        /// </summary>
        /// <param name="value">The boolean value to convert back</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>The inverted boolean value or null if input is not boolean</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return null;
        }
    }
} 