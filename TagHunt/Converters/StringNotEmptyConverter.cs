using System.Globalization;

namespace TagHunt.Converters
{
    /// <summary>
    /// Value converter that checks if a string is not null, empty, or whitespace
    /// </summary>
    public class StringNotEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string value to a boolean indicating whether it's not empty
        /// </summary>
        /// <param name="value">The string value to check</param>
        /// <param name="targetType">The target type for conversion</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>True if the string is not null, empty, or whitespace; false otherwise</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrWhiteSpace(stringValue);
            }
            return false;
        }

        /// <summary>
        /// Converts back from boolean to string (not implemented as it's not needed)
        /// </summary>
        /// <param name="value">The value to convert back</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Throws NotImplementedException</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 